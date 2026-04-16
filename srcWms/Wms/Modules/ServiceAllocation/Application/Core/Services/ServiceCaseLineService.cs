using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.ServiceAllocation.Enums;
using OrderAllocationLineEntity = Wms.Domain.Entities.ServiceAllocation.OrderAllocationLine;
using ServiceCaseEntity = Wms.Domain.Entities.ServiceAllocation.ServiceCase;
using ServiceCaseLineEntity = Wms.Domain.Entities.ServiceAllocation.ServiceCaseLine;

namespace Wms.Application.ServiceAllocation.Services;

public sealed class ServiceCaseLineService : IServiceCaseLineService
{
    private readonly IRepository<ServiceCaseLineEntity> _serviceCaseLines;
    private readonly IRepository<ServiceCaseEntity> _serviceCases;
    private readonly IRepository<OrderAllocationLineEntity> _allocationLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public ServiceCaseLineService(
        IRepository<ServiceCaseLineEntity> serviceCaseLines,
        IRepository<ServiceCaseEntity> serviceCases,
        IRepository<OrderAllocationLineEntity> allocationLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper,
        IEntityReferenceResolver entityReferenceResolver)
    {
        _serviceCaseLines = serviceCaseLines;
        _serviceCases = serviceCases;
        _allocationLines = allocationLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<ServiceCaseLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await BuildQuery().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<ServiceCaseLineDto>>.SuccessResult(_mapper.Map<List<ServiceCaseLineDto>>(items), _localizationService.GetLocalizedString("ServiceCaseLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<ServiceCaseLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = BuildQuery()
            .ApplySearch(request.Search,
                nameof(ServiceCaseLineEntity.StockCode),
                nameof(ServiceCaseLineEntity.ErpOrderNo),
                nameof(ServiceCaseLineEntity.ErpOrderId),
                nameof(ServiceCaseLineEntity.Description))
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? nameof(ServiceCaseLineEntity.Id), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtoItems = _mapper.Map<List<ServiceCaseLineDto>>(items);
        var page = new PagedResponse<ServiceCaseLineDto>(dtoItems, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<ServiceCaseLineDto>>.SuccessResult(page, _localizationService.GetLocalizedString("ServiceCaseLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ServiceCaseLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceCaseLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseLineNotFound");
            return ApiResponse<ServiceCaseLineDto>.ErrorResult(message, message, 404);
        }

        return ApiResponse<ServiceCaseLineDto>.SuccessResult(_mapper.Map<ServiceCaseLineDto>(entity), _localizationService.GetLocalizedString("ServiceCaseLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<ServiceCaseLineDto>> CreateAsync(CreateServiceCaseLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<ServiceCaseLineEntity>(createDto) ?? new ServiceCaseLineEntity();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        var validationError = ValidateEntity(entity);
        if (validationError != null)
        {
            return ApiResponse<ServiceCaseLineDto>.ErrorResult(validationError, validationError, 400);
        }
        Normalize(entity, createDto.BranchCode);
        entity.CreatedDate = DateTimeProvider.Now;
        await _serviceCaseLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await SyncAllocationAsync(entity, cancellationToken);

        return ApiResponse<ServiceCaseLineDto>.SuccessResult(_mapper.Map<ServiceCaseLineDto>(entity), _localizationService.GetLocalizedString("ServiceCaseLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<ServiceCaseLineDto>> UpdateAsync(long id, UpdateServiceCaseLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceCaseLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseLineNotFound");
            return ApiResponse<ServiceCaseLineDto>.ErrorResult(message, message, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        var validationError = ValidateEntity(entity);
        if (validationError != null)
        {
            return ApiResponse<ServiceCaseLineDto>.ErrorResult(validationError, validationError, 400);
        }
        Normalize(entity, updateDto.BranchCode);
        entity.UpdatedDate = DateTimeProvider.Now;
        _serviceCaseLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await SyncAllocationAsync(entity, cancellationToken);
        return ApiResponse<ServiceCaseLineDto>.SuccessResult(_mapper.Map<ServiceCaseLineDto>(entity), _localizationService.GetLocalizedString("ServiceCaseLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceCaseLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("ServiceCaseLineNotFound");
            return ApiResponse<bool>.ErrorResult(message, message, 404);
        }

        entity.IsDeleted = true;
        entity.UpdatedDate = DateTimeProvider.Now;
        _serviceCaseLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await SyncAllocationAsync(entity, cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ServiceCaseLineDeletedSuccessfully"));
    }

    private IQueryable<ServiceCaseLineEntity> BuildQuery() => _serviceCaseLines.Query().Where(x => !x.IsDeleted);

    private static void Normalize(ServiceCaseLineEntity entity, string? branchCode)
    {
        entity.StockCode = NormalizeOptionalCode(entity.StockCode);
        entity.ErpOrderNo = NormalizeOptionalCode(entity.ErpOrderNo);
        entity.ErpOrderId = string.IsNullOrWhiteSpace(entity.ErpOrderId) ? null : entity.ErpOrderId.Trim();
        entity.Unit = string.IsNullOrWhiteSpace(entity.Unit) ? null : entity.Unit.Trim();
        entity.Description = string.IsNullOrWhiteSpace(entity.Description) ? null : entity.Description.Trim();
        entity.BranchCode = NormalizeBranchCode(branchCode ?? entity.BranchCode);
    }

    private static string? NormalizeOptionalCode(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
    private static string NormalizeBranchCode(string? branchCode) => string.IsNullOrWhiteSpace(branchCode) ? "0" : branchCode.Trim();

    private string? ValidateEntity(ServiceCaseLineEntity entity)
    {
        if (entity.ServiceCaseId <= 0)
        {
            return _localizationService.GetLocalizedString("ServiceCaseIdRequired");
        }

        if (entity.Quantity <= 0)
        {
            return _localizationService.GetLocalizedString("QuantityMustBeGreaterThanZero");
        }

        var requiresStock = entity.LineType is Wms.Domain.Entities.ServiceAllocation.Enums.ServiceCaseLineType.SparePart
            or Wms.Domain.Entities.ServiceAllocation.Enums.ServiceCaseLineType.ReplacementProduct;

        if (requiresStock && (!entity.StockId.HasValue || entity.StockId.Value <= 0))
        {
            return _localizationService.GetLocalizedString("StockIdRequiredForMaterialLine");
        }

        if (entity.LineType == Wms.Domain.Entities.ServiceAllocation.Enums.ServiceCaseLineType.Labor)
        {
            entity.StockId = null;
            entity.StockCode = null;
        }

        return null;
    }

    private async Task SyncAllocationAsync(ServiceCaseLineEntity line, CancellationToken cancellationToken)
    {
        if (line.ServiceCaseId <= 0)
        {
            return;
        }

        if (line.LineType == ServiceCaseLineType.Labor || !line.StockId.HasValue || line.StockId.Value <= 0)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(line.ErpOrderId) || string.IsNullOrWhiteSpace(line.ErpOrderNo))
        {
            return;
        }

        var serviceCase = await _serviceCases.Query()
            .Where(x => !x.IsDeleted && x.Id == line.ServiceCaseId)
            .Select(x => new { x.Id, x.CustomerCode, x.CustomerId, x.BranchCode })
            .FirstOrDefaultAsync(cancellationToken);

        if (serviceCase == null)
        {
            return;
        }

        var totalRequested = await _serviceCaseLines.Query()
            .Where(x => !x.IsDeleted
                && x.ServiceCaseId == line.ServiceCaseId
                && x.StockId == line.StockId
                && x.ProcessType == line.ProcessType
                && x.LineType != ServiceCaseLineType.Labor
                && x.ErpOrderId == line.ErpOrderId)
            .SumAsync(x => x.Quantity, cancellationToken);

        var existing = await _allocationLines.Query(tracking: true)
            .Where(x => !x.IsDeleted
                && x.StockId == line.StockId.Value
                && x.ProcessType == line.ProcessType
                && x.ErpOrderId == line.ErpOrderId)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (totalRequested <= 0)
        {
            if (existing != null)
            {
                existing.RequestedQuantity = 0;
                existing.AllocatedQuantity = 0;
                existing.ReservedQuantity = 0;
                existing.Status = AllocationStatus.Cancelled;
                existing.UpdatedDate = DateTimeProvider.Now;
                _allocationLines.Update(existing);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return;
        }

        if (existing == null)
        {
            existing = new OrderAllocationLineEntity
            {
                BranchCode = serviceCase.BranchCode,
                StockId = line.StockId.Value,
                StockCode = line.StockCode ?? string.Empty,
                ErpOrderNo = line.ErpOrderNo ?? string.Empty,
                ErpOrderId = line.ErpOrderId ?? string.Empty,
                CustomerCode = serviceCase.CustomerCode,
                CustomerId = serviceCase.CustomerId,
                ProcessType = line.ProcessType,
                RequestedQuantity = totalRequested,
                AllocatedQuantity = 0,
                ReservedQuantity = 0,
                FulfilledQuantity = 0,
                PriorityNo = 0,
                Status = AllocationStatus.Waiting,
                CreatedDate = DateTimeProvider.Now
            };

            await _allocationLines.AddAsync(existing, cancellationToken);
        }
        else
        {
            existing.StockCode = line.StockCode ?? existing.StockCode;
            existing.ErpOrderNo = line.ErpOrderNo ?? existing.ErpOrderNo;
            existing.CustomerCode = serviceCase.CustomerCode;
            existing.CustomerId = serviceCase.CustomerId;
            existing.RequestedQuantity = totalRequested;
            existing.Status = existing.Status == AllocationStatus.Cancelled ? AllocationStatus.Waiting : existing.Status;
            existing.UpdatedDate = DateTimeProvider.Now;
            _allocationLines.Update(existing);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
