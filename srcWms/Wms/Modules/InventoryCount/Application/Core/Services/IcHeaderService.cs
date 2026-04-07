using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;
using StockEntity = Wms.Domain.Entities.Stock.Stock;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcHeaderService : IIcHeaderService
{
    private readonly IRepository<IcHeader> _headers;
    private readonly IRepository<IcImportLine> _importLines;
    private readonly IRepository<IcLine> _lines;
    private readonly IRepository<IcScope> _scopes;
    private readonly IRepository<StockEntity> _stocks;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public IcHeaderService(
        IRepository<IcHeader> headers,
        IRepository<IcImportLine> importLines,
        IRepository<IcLine> lines,
        IRepository<IcScope> scopes,
        IRepository<StockEntity> stocks,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        IEntityReferenceResolver entityReferenceResolver)
    {
        _headers = headers;
        _importLines = importLines;
        _lines = lines;
        _scopes = scopes;
        _stocks = stocks;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<IcHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        => ApiResponse<IEnumerable<IcHeaderDto>>.SuccessResult(_mapper.Map<List<IcHeaderDto>>(await _headers.Query().ToListAsync(cancellationToken)), _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));

    public async Task<ApiResponse<PagedResponse<IcHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query()
            .Where(x => !x.IsDeleted)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<IcHeaderDto>>.SuccessResult(new PagedResponse<IcHeaderDto>(_mapper.Map<List<IcHeaderDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<IcHeaderDto>>> GetAssignedPagedAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var query = _headers.Query()
            .Where(x => !x.IsDeleted && x.AssignedUserId == userId)
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(cancellationToken);
        return ApiResponse<PagedResponse<IcHeaderDto>>.SuccessResult(new PagedResponse<IcHeaderDto>(_mapper.Map<List<IcHeaderDto>>(items), total, request.PageNumber < 1 ? 1 : request.PageNumber, request.PageSize < 1 ? 20 : request.PageSize), _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<IcHeaderDto>.ErrorResult(msg, msg, 404);
        }
        return ApiResponse<IcHeaderDto>.SuccessResult(_mapper.Map<IcHeaderDto>(entity), _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcHeaderDto>> CreateAsync(CreateIcHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<IcHeader>(createDto) ?? new IcHeader();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;
        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcHeaderDto>.SuccessResult(_mapper.Map<IcHeaderDto>(entity), _localizationService.GetLocalizedString("IcHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IcHeaderDto>> UpdateAsync(long id, UpdateIcHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity == null || entity.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<IcHeaderDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcHeaderDto>.SuccessResult(_mapper.Map<IcHeaderDto>(entity), _localizationService.GetLocalizedString("IcHeaderUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> GenerateLinesAsync(long id, CancellationToken cancellationToken = default)
    {
        var header = await _headers.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (header == null)
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        if (await _lines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderLinesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        var scopes = await _scopes.Query().Where(x => x.HeaderId == id && !x.IsDeleted && x.IsActive).OrderBy(x => x.SequenceNo).ToListAsync(cancellationToken);
        var seeds = new List<(long? ScopeId, long? WarehouseId, string? WarehouseCode, string? RackCode, string? CellCode, long? StockId, string StockCode, long? YapKodId, string? YapKod, string? Unit)>();

        async Task addStockSeedsAsync(long? scopeId, long? warehouseId, string? warehouseCode, string? rackCode, string? cellCode, long? stockId, string? stockCode, long? yapKodId, string? yapKod)
        {
            if (stockId.HasValue || !string.IsNullOrWhiteSpace(stockCode))
            {
                var stockEntity = stockId.HasValue
                    ? await _stocks.Query().FirstOrDefaultAsync(x => x.Id == stockId.Value && !x.IsDeleted, cancellationToken)
                    : await _stocks.Query().FirstOrDefaultAsync(x => x.ErpStockCode == stockCode && !x.IsDeleted, cancellationToken);

                if (stockEntity != null)
                {
                    seeds.Add((scopeId, warehouseId, warehouseCode, rackCode, cellCode, stockEntity.Id, stockEntity.ErpStockCode, yapKodId, yapKod, stockEntity.Unit));
                }
                else if (!string.IsNullOrWhiteSpace(stockCode))
                {
                    seeds.Add((scopeId, warehouseId, warehouseCode, rackCode, cellCode, stockId, stockCode, yapKodId, yapKod, null));
                }
                return;
            }

            var stocks = await _stocks.Query().Where(x => !x.IsDeleted).OrderBy(x => x.ErpStockCode).ToListAsync(cancellationToken);
            foreach (var stock in stocks)
            {
                seeds.Add((scopeId, warehouseId, warehouseCode, rackCode, cellCode, stock.Id, stock.ErpStockCode, yapKodId, yapKod, stock.Unit));
            }
        }

        if (header.ScopeMode == "Mixed" && scopes.Count > 0)
        {
            foreach (var scope in scopes)
            {
                await addStockSeedsAsync(scope.Id, scope.WarehouseId, scope.WarehouseCode, scope.RackCode, scope.CellCode, scope.StockId, scope.StockCode, scope.YapKodId, scope.YapKod);
            }
        }
        else
        {
            await addStockSeedsAsync(null, header.WarehouseId, header.WarehouseCode, header.RackCode, header.CellCode, header.StockId, header.StockCode, header.YapKodId, header.YapKod);
        }

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var sequence = 1;
        foreach (var seed in seeds)
        {
            var key = string.Join('|', seed.ScopeId?.ToString() ?? "0", seed.WarehouseCode ?? "", seed.RackCode ?? "", seed.CellCode ?? "", seed.StockCode, seed.YapKod ?? "");
            if (!seen.Add(key))
            {
                continue;
            }

            var line = new IcLine
            {
                HeaderId = header.Id,
                ScopeId = seed.ScopeId,
                SequenceNo = sequence++,
                WarehouseId = seed.WarehouseId,
                WarehouseCode = seed.WarehouseCode,
                RackCode = seed.RackCode,
                CellCode = seed.CellCode,
                StockId = seed.StockId ?? 0,
                StockCode = seed.StockCode,
                YapKodId = seed.YapKodId,
                YapKod = seed.YapKod,
                Unit = seed.Unit,
                ExpectedQuantity = 0m,
                CountStatus = "Pending",
                CreatedDate = DateTimeProvider.Now,
                IsDeleted = false,
            };

            await _entityReferenceResolver.ResolveAsync(line, cancellationToken);
            await _lines.AddAsync(line, cancellationToken);
        }

        header.LineCount = sequence - 1;
        header.CountedLineCount = 0;
        header.DifferenceLineCount = 0;
        header.RecountRequiredLineCount = 0;
        header.Status = (sequence - 1) > 0 ? "Released" : header.Status;
        header.UpdatedDate = DateTimeProvider.Now;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcHeaderLinesGeneratedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        if (!await _headers.ExistsAsync(id, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }
        if (await _importLines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken)
            || await _lines.Query().AnyAsync(x => x.HeaderId == id && !x.IsDeleted, cancellationToken))
        {
            var msg = _localizationService.GetLocalizedString("IcHeaderImportLinesExist");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }
        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcHeaderDeletedSuccessfully"));
    }
}
