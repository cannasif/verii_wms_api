using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;
using BusinessDocumentLinkEntity = Wms.Domain.Entities.ServiceAllocation.BusinessDocumentLink;
using OrderAllocationLineEntity = Wms.Domain.Entities.ServiceAllocation.OrderAllocationLine;

namespace Wms.Application.ServiceAllocation.Services;

public sealed class AllocationEngine : IAllocationEngine
{
    private readonly IRepository<OrderAllocationLineEntity> _allocationLines;
    private readonly IRepository<BusinessDocumentLinkEntity> _documentLinks;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public AllocationEngine(
        IRepository<OrderAllocationLineEntity> allocationLines,
        IRepository<BusinessDocumentLinkEntity> documentLinks,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService)
    {
        _allocationLines = allocationLines;
        _documentLinks = documentLinks;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
    }

    public async Task<ApiResponse<AllocationRecomputeResultDto>> RecomputeAsync(RecomputeAllocationRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request.StockId <= 0)
        {
            var message = _localizationService.GetLocalizedString("StockIdRequired");
            return ApiResponse<AllocationRecomputeResultDto>.ErrorResult(message, message, 400);
        }

        if (request.AvailableQuantity < 0)
        {
            var message = _localizationService.GetLocalizedString("AvailableQuantityCannotBeNegative");
            return ApiResponse<AllocationRecomputeResultDto>.ErrorResult(message, message, 400);
        }

        var lines = await _allocationLines.Query(tracking: true)
            .Where(x => !x.IsDeleted && x.StockId == request.StockId)
            .OrderBy(x => x.PriorityNo)
            .ThenBy(x => x.CreatedDate ?? DateTime.MinValue)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        var remaining = request.AvailableQuantity;
        var result = new AllocationRecomputeResultDto
        {
            StockId = request.StockId,
            AvailableQuantity = request.AvailableQuantity
        };

        foreach (var line in lines)
        {
            if (line.Status == AllocationStatus.Cancelled || line.Status == AllocationStatus.Blocked)
            {
                result.Lines.Add(ToResult(line));
                continue;
            }

            var remainingDemand = Math.Max(0, line.RequestedQuantity - line.FulfilledQuantity);
            if (remainingDemand <= 0)
            {
                line.AllocatedQuantity = 0;
                line.Status = AllocationStatus.Shipped;
                line.UpdatedDate = DateTimeProvider.Now;
                result.Lines.Add(ToResult(line));
                continue;
            }

            var allocated = Math.Min(remainingDemand, remaining);
            line.AllocatedQuantity = allocated;
            line.Status = ResolveStatus(remainingDemand, allocated);
            line.UpdatedDate = DateTimeProvider.Now;
            remaining -= allocated;

            await EnsureSourceDocumentLinkAsync(line, cancellationToken);

            result.Lines.Add(ToResult(line));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        result.ProcessedLineCount = result.Lines.Count;
        result.RemainingQuantity = remaining;
        return ApiResponse<AllocationRecomputeResultDto>.SuccessResult(result, _localizationService.GetLocalizedString("AllocationRecomputedSuccessfully"));
    }

    private async Task EnsureSourceDocumentLinkAsync(OrderAllocationLineEntity line, CancellationToken cancellationToken)
    {
        if (line.SourceHeaderId is null || string.IsNullOrWhiteSpace(line.SourceModule))
        {
            return;
        }

        if (!TryResolveDocumentModule(line.SourceModule, out var documentModule))
        {
            return;
        }

        var exists = await _documentLinks.Query().AnyAsync(
            x => !x.IsDeleted
                && x.BusinessEntityType == BusinessEntityType.OrderAllocationLine
                && x.BusinessEntityId == line.Id
                && x.OrderAllocationLineId == line.Id
                && x.DocumentModule == documentModule
                && x.DocumentHeaderId == line.SourceHeaderId.Value
                && x.DocumentLineId == line.SourceLineId
                && x.LinkPurpose == DocumentLinkPurpose.AllocationSource,
            cancellationToken);

        if (exists)
        {
            return;
        }

        var link = new BusinessDocumentLinkEntity
        {
            BranchCode = line.BranchCode,
            BusinessEntityType = BusinessEntityType.OrderAllocationLine,
            BusinessEntityId = line.Id,
            OrderAllocationLineId = line.Id,
            DocumentModule = documentModule,
            DocumentHeaderId = line.SourceHeaderId.Value,
            DocumentLineId = line.SourceLineId,
            LinkPurpose = DocumentLinkPurpose.AllocationSource,
            SequenceNo = 0,
            LinkedAt = DateTimeProvider.Now
        };

        await _documentLinks.AddAsync(link, cancellationToken);
    }

    private static bool TryResolveDocumentModule(string sourceModule, out DocumentModule documentModule)
    {
        return Enum.TryParse(sourceModule.Trim(), true, out documentModule);
    }

    private static AllocationStatus ResolveStatus(decimal remainingDemand, decimal allocated)
    {
        if (allocated <= 0)
        {
            return AllocationStatus.Waiting;
        }

        if (allocated < remainingDemand)
        {
            return AllocationStatus.PartiallyAllocated;
        }

        return AllocationStatus.Allocated;
    }

    private static AllocationRecomputeLineResultDto ToResult(OrderAllocationLineEntity line)
    {
        return new AllocationRecomputeLineResultDto
        {
            AllocationLineId = line.Id,
            ErpOrderNo = line.ErpOrderNo,
            ErpOrderId = line.ErpOrderId,
            RequestedQuantity = line.RequestedQuantity,
            FulfilledQuantity = line.FulfilledQuantity,
            AllocatedQuantity = line.AllocatedQuantity,
            Status = line.Status,
            PriorityNo = line.PriorityNo
        };
    }
}
