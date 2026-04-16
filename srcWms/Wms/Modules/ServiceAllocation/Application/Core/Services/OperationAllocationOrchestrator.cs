using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Application.System.Services;
using Wms.Domain.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;
using BusinessDocumentLinkEntity = Wms.Domain.Entities.ServiceAllocation.BusinessDocumentLink;
using OrderAllocationLineEntity = Wms.Domain.Entities.ServiceAllocation.OrderAllocationLine;
using ServiceCaseLineEntity = Wms.Domain.Entities.ServiceAllocation.ServiceCaseLine;
using StockEntity = Wms.Domain.Entities.Stock.Stock;

namespace Wms.Application.ServiceAllocation.Services;

public sealed class OperationAllocationOrchestrator : IOperationAllocationOrchestrator
{
    private sealed record AllocationReference(long Id, long StockId, string ErpOrderNo, string ErpOrderId);

    private readonly IRepository<OrderAllocationLineEntity> _allocationLines;
    private readonly IRepository<ServiceCaseLineEntity> _serviceCaseLines;
    private readonly IRepository<BusinessDocumentLinkEntity> _documentLinks;
    private readonly IRepository<StockEntity> _stocks;
    private readonly IErpService _erpService;
    private readonly IAllocationEngine _allocationEngine;
    private readonly IUnitOfWork _unitOfWork;

    public OperationAllocationOrchestrator(
        IRepository<OrderAllocationLineEntity> allocationLines,
        IRepository<ServiceCaseLineEntity> serviceCaseLines,
        IRepository<BusinessDocumentLinkEntity> documentLinks,
        IRepository<StockEntity> stocks,
        IErpService erpService,
        IAllocationEngine allocationEngine,
        IUnitOfWork unitOfWork)
    {
        _allocationLines = allocationLines;
        _serviceCaseLines = serviceCaseLines;
        _documentLinks = documentLinks;
        _stocks = stocks;
        _erpService = erpService;
        _allocationEngine = allocationEngine;
        _unitOfWork = unitOfWork;
    }

    public async Task TriggerForDocumentAsync(DocumentModule documentModule, long documentHeaderId, CancellationToken cancellationToken = default)
    {
        if (documentHeaderId <= 0)
        {
            return;
        }

        var sourceModule = documentModule.ToString();
        var allocations = await _allocationLines.Query()
            .Where(x => !x.IsDeleted
                && x.SourceHeaderId == documentHeaderId
                && x.SourceModule == sourceModule
                && x.StockId > 0)
            .Select(x => new AllocationReference(x.Id, x.StockId, x.ErpOrderNo, x.ErpOrderId))
            .ToListAsync(cancellationToken);

        var stockIds = allocations.Select(x => x.StockId).Distinct().ToList();
        foreach (var stockId in stockIds)
        {
            var stock = await _stocks.Query()
                .Where(x => !x.IsDeleted && x.Id == stockId)
                .Select(x => new { x.Id, x.ErpStockCode })
                .FirstOrDefaultAsync(cancellationToken);

            if (stock == null || string.IsNullOrWhiteSpace(stock.ErpStockCode))
            {
                continue;
            }

            var onHand = await _erpService.GetOnHandQuantitiesAsync(stokKodu: stock.ErpStockCode, cancellationToken: cancellationToken);
            if (!onHand.Success || onHand.Data == null)
            {
                continue;
            }

            var availableQuantity = onHand.Data.Sum(x => x.Bakiye ?? 0m);
            await _allocationEngine.RecomputeAsync(
                new RecomputeAllocationRequestDto
                {
                    StockId = stock.Id,
                    AvailableQuantity = availableQuantity
                },
                cancellationToken);
        }

        await EnsureServiceCaseLinksAsync(documentModule, documentHeaderId, allocations, cancellationToken);
    }

    private async Task EnsureServiceCaseLinksAsync(
        DocumentModule documentModule,
        long documentHeaderId,
        IReadOnlyCollection<AllocationReference> allocations,
        CancellationToken cancellationToken)
    {
        var pendingLinks = new List<BusinessDocumentLinkEntity>();
        var handledCases = new HashSet<long>();

        foreach (var allocation in allocations)
        {
            var erpOrderId = allocation.ErpOrderId;
            var erpOrderNo = allocation.ErpOrderNo;

            if (string.IsNullOrWhiteSpace(erpOrderId) && string.IsNullOrWhiteSpace(erpOrderNo))
            {
                continue;
            }

            var serviceCaseIds = await _serviceCaseLines.Query()
                .Where(x => !x.IsDeleted
                    && ((!string.IsNullOrWhiteSpace(erpOrderId) && x.ErpOrderId == erpOrderId)
                        || (!string.IsNullOrWhiteSpace(erpOrderNo) && x.ErpOrderNo == erpOrderNo)))
                .Select(x => x.ServiceCaseId)
                .Distinct()
                .ToListAsync(cancellationToken);

            foreach (var serviceCaseId in serviceCaseIds)
            {
                if (!handledCases.Add(serviceCaseId))
                {
                    continue;
                }

                var exists = await _documentLinks.Query().AnyAsync(
                    x => !x.IsDeleted
                        && x.BusinessEntityType == BusinessEntityType.ServiceCase
                        && x.BusinessEntityId == serviceCaseId
                        && x.ServiceCaseId == serviceCaseId
                        && x.DocumentModule == documentModule
                        && x.DocumentHeaderId == documentHeaderId,
                    cancellationToken);

                if (exists)
                {
                    continue;
                }

                var sequenceNo = (await _documentLinks.Query()
                    .Where(x => !x.IsDeleted
                        && x.BusinessEntityType == BusinessEntityType.ServiceCase
                        && x.BusinessEntityId == serviceCaseId)
                    .MaxAsync(x => (int?)x.SequenceNo, cancellationToken) ?? 0) + 1;

                pendingLinks.Add(new BusinessDocumentLinkEntity
                {
                    BusinessEntityType = BusinessEntityType.ServiceCase,
                    BusinessEntityId = serviceCaseId,
                    ServiceCaseId = serviceCaseId,
                    DocumentModule = documentModule,
                    DocumentHeaderId = documentHeaderId,
                    LinkPurpose = ResolveLinkPurpose(documentModule),
                    SequenceNo = sequenceNo,
                    LinkedAt = DateTimeProvider.Now,
                    Note = BuildNote(documentModule, erpOrderNo, erpOrderId)
                });
            }
        }

        if (pendingLinks.Count == 0)
        {
            return;
        }

        await _documentLinks.AddRangeAsync(pendingLinks, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static DocumentLinkPurpose ResolveLinkPurpose(DocumentModule documentModule)
    {
        return documentModule switch
        {
            DocumentModule.WI => DocumentLinkPurpose.Intake,
            DocumentModule.WT => DocumentLinkPurpose.InternalTransfer,
            DocumentModule.WO => DocumentLinkPurpose.SparePartSupply,
            DocumentModule.SH => DocumentLinkPurpose.Shipment,
            DocumentModule.SIT => DocumentLinkPurpose.RepairOperation,
            DocumentModule.SRT => DocumentLinkPurpose.ReturnToMainWarehouse,
            _ => DocumentLinkPurpose.AllocationResult
        };
    }

    private static string BuildNote(DocumentModule documentModule, string? erpOrderNo, string? erpOrderId)
    {
        var orderNo = string.IsNullOrWhiteSpace(erpOrderNo) ? "-" : erpOrderNo;
        var orderId = string.IsNullOrWhiteSpace(erpOrderId) ? "-" : erpOrderId;
        return $"Auto linked from {documentModule} for ERP order {orderNo} / {orderId}";
    }
}
