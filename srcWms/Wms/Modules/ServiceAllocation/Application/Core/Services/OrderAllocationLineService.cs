using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;
using Wms.Domain.Common;
using OrderAllocationLineEntity = Wms.Domain.Entities.ServiceAllocation.OrderAllocationLine;

namespace Wms.Application.ServiceAllocation.Services;

public sealed class OrderAllocationLineService : IOrderAllocationLineService
{
    private readonly IRepository<OrderAllocationLineEntity> _allocationLines;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IMapper _mapper;
    private readonly IEntityReferenceResolver _entityReferenceResolver;
    private readonly IAllocationEngine _allocationEngine;

    public OrderAllocationLineService(
        IRepository<OrderAllocationLineEntity> allocationLines,
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IMapper mapper,
        IEntityReferenceResolver entityReferenceResolver,
        IAllocationEngine allocationEngine)
    {
        _allocationLines = allocationLines;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _mapper = mapper;
        _entityReferenceResolver = entityReferenceResolver;
        _allocationEngine = allocationEngine;
    }

    public async Task<ApiResponse<IEnumerable<OrderAllocationLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await BuildQuery().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<OrderAllocationLineDto>>.SuccessResult(_mapper.Map<List<OrderAllocationLineDto>>(items), _localizationService.GetLocalizedString("OrderAllocationLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<OrderAllocationLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = BuildQuery()
            .ApplySearch(request.Search,
                nameof(OrderAllocationLineEntity.StockCode),
                nameof(OrderAllocationLineEntity.ErpOrderNo),
                nameof(OrderAllocationLineEntity.ErpOrderId),
                nameof(OrderAllocationLineEntity.CustomerCode),
                nameof(OrderAllocationLineEntity.SourceModule))
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? nameof(OrderAllocationLineEntity.Id), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));

        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        var dtoItems = _mapper.Map<List<OrderAllocationLineDto>>(items);
        var page = new PagedResponse<OrderAllocationLineDto>(dtoItems, total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize);
        return ApiResponse<PagedResponse<OrderAllocationLineDto>>.SuccessResult(page, _localizationService.GetLocalizedString("OrderAllocationLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<OrderAllocationLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _allocationLines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("OrderAllocationLineNotFound");
            return ApiResponse<OrderAllocationLineDto>.ErrorResult(message, message, 404);
        }

        return ApiResponse<OrderAllocationLineDto>.SuccessResult(_mapper.Map<OrderAllocationLineDto>(entity), _localizationService.GetLocalizedString("OrderAllocationLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<OrderAllocationLineDto>> CreateAsync(CreateOrderAllocationLineDto createDto, CancellationToken cancellationToken = default)
    {
        if (!createDto.StockId.HasValue || createDto.StockId.Value <= 0)
        {
            var message = _localizationService.GetLocalizedString("StockIdRequired");
            return ApiResponse<OrderAllocationLineDto>.ErrorResult(message, message, 400);
        }

        var duplicate = await _allocationLines.Query().AnyAsync(
            x => x.ErpOrderId == NormalizeOrderKey(createDto.ErpOrderId)
                && x.StockId == createDto.StockId.Value
                && x.SourceLineId == createDto.SourceLineId
                && !x.IsDeleted,
            cancellationToken);
        if (duplicate)
        {
            var message = _localizationService.GetLocalizedString("OrderAllocationLineAlreadyExists");
            return ApiResponse<OrderAllocationLineDto>.ErrorResult(message, message, 400);
        }

        var entity = _mapper.Map<OrderAllocationLineEntity>(createDto) ?? new OrderAllocationLineEntity();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        if (entity.StockId <= 0)
        {
            var message = _localizationService.GetLocalizedString("StockIdRequired");
            return ApiResponse<OrderAllocationLineDto>.ErrorResult(message, message, 400);
        }
        Normalize(entity, createDto.BranchCode);
        entity.CreatedDate = DateTimeProvider.Now;
        await _allocationLines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<OrderAllocationLineDto>.SuccessResult(_mapper.Map<OrderAllocationLineDto>(entity), _localizationService.GetLocalizedString("OrderAllocationLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<OrderAllocationLineDto>> UpdateAsync(long id, UpdateOrderAllocationLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _allocationLines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var message = _localizationService.GetLocalizedString("OrderAllocationLineNotFound");
            return ApiResponse<OrderAllocationLineDto>.ErrorResult(message, message, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        if (entity.StockId <= 0)
        {
            var message = _localizationService.GetLocalizedString("StockIdRequired");
            return ApiResponse<OrderAllocationLineDto>.ErrorResult(message, message, 400);
        }
        Normalize(entity, updateDto.BranchCode);
        entity.UpdatedDate = DateTimeProvider.Now;
        _allocationLines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<OrderAllocationLineDto>.SuccessResult(_mapper.Map<OrderAllocationLineDto>(entity), _localizationService.GetLocalizedString("OrderAllocationLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _allocationLines.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var message = _localizationService.GetLocalizedString("OrderAllocationLineNotFound");
            return ApiResponse<bool>.ErrorResult(message, message, 404);
        }

        await _allocationLines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OrderAllocationLineDeletedSuccessfully"));
    }

    public Task<ApiResponse<AllocationRecomputeResultDto>> RecomputeAsync(RecomputeAllocationRequestDto request, CancellationToken cancellationToken = default)
    {
        return _allocationEngine.RecomputeAsync(request, cancellationToken);
    }

    private IQueryable<OrderAllocationLineEntity> BuildQuery() => _allocationLines.Query().Where(x => !x.IsDeleted);

    private static void Normalize(OrderAllocationLineEntity entity, string? branchCode)
    {
        entity.StockCode = NormalizeCode(entity.StockCode);
        entity.ErpOrderNo = NormalizeOrderNo(entity.ErpOrderNo);
        entity.ErpOrderId = NormalizeOrderKey(entity.ErpOrderId);
        entity.CustomerCode = NormalizeCode(entity.CustomerCode);
        entity.SourceModule = string.IsNullOrWhiteSpace(entity.SourceModule) ? null : entity.SourceModule.Trim().ToUpperInvariant();
        entity.BranchCode = NormalizeBranchCode(branchCode ?? entity.BranchCode);
    }

    private static string NormalizeCode(string value) => value.Trim().ToUpperInvariant();
    private static string NormalizeOrderNo(string value) => value.Trim().ToUpperInvariant();
    private static string NormalizeOrderKey(string value) => value.Trim();
    private static string NormalizeBranchCode(string? branchCode) => string.IsNullOrWhiteSpace(branchCode) ? "0" : branchCode.Trim();
}
