using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Package.Dtos;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using YapKodEntity = Wms.Domain.Entities.YapKod.YapKod;

namespace Wms.Application.Package.Services;

public sealed class PLineService : IPLineService
{
    private readonly IRepository<PLine> _lines;
    private readonly IRepository<PHeader> _headers;
    private readonly IRepository<PPackage> _packages;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly IPackageGoodsReceiptMatcher _goodsReceiptMatcher;
    private readonly IPackageWarehouseTransferMatcher _warehouseTransferMatcher;
    private readonly IPackageWarehouseOutboundMatcher _warehouseOutboundMatcher;
    private readonly IPackageWarehouseInboundMatcher _warehouseInboundMatcher;
    private readonly IPackageShippingMatcher _shippingMatcher;
    private readonly IPackageProductionMatcher _productionMatcher;
    private readonly IPackageProductionTransferMatcher _productionTransferMatcher;
    private readonly IPackageSubcontractingIssueMatcher _subcontractingIssueMatcher;
    private readonly IPackageSubcontractingReceiptMatcher _subcontractingReceiptMatcher;
    private readonly IEntityReferenceResolver _entityReferenceResolver;
    private readonly IRepository<YapKodEntity> _yapKodlar;

    public PLineService(
        IRepository<PLine> lines,
        IRepository<PHeader> headers,
        IRepository<PPackage> packages,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        IPackageGoodsReceiptMatcher goodsReceiptMatcher,
        IPackageWarehouseTransferMatcher warehouseTransferMatcher,
        IPackageWarehouseOutboundMatcher warehouseOutboundMatcher,
        IPackageWarehouseInboundMatcher warehouseInboundMatcher,
        IPackageShippingMatcher shippingMatcher,
        IPackageProductionMatcher productionMatcher,
        IPackageProductionTransferMatcher productionTransferMatcher,
        IPackageSubcontractingIssueMatcher subcontractingIssueMatcher,
        IPackageSubcontractingReceiptMatcher subcontractingReceiptMatcher,
        IEntityReferenceResolver entityReferenceResolver,
        IRepository<YapKodEntity> yapKodlar)
    {
        _lines = lines;
        _headers = headers;
        _packages = packages;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _goodsReceiptMatcher = goodsReceiptMatcher;
        _warehouseTransferMatcher = warehouseTransferMatcher;
        _warehouseOutboundMatcher = warehouseOutboundMatcher;
        _warehouseInboundMatcher = warehouseInboundMatcher;
        _shippingMatcher = shippingMatcher;
        _productionMatcher = productionMatcher;
        _productionTransferMatcher = productionTransferMatcher;
        _subcontractingIssueMatcher = subcontractingIssueMatcher;
        _subcontractingReceiptMatcher = subcontractingReceiptMatcher;
        _entityReferenceResolver = entityReferenceResolver;
        _yapKodlar = yapKodlar;
    }

    public async Task<ApiResponse<IEnumerable<PLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PLineDto>>(items);
        await EnrichDtosAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var query = _lines.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(pageNumber, pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PLineDto>>(items);
        await EnrichDtosAsync(dtos, cancellationToken);
        return ApiResponse<PagedResponse<PLineDto>>.SuccessResult(new PagedResponse<PLineDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PLineNotFound");
            return ApiResponse<PLineDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PLineDto>(entity);
        await EnrichDtoAsync(dto, cancellationToken);
        return ApiResponse<PLineDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackageIdAsync(long packageId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.PackageId == packageId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PLineDto>>(items);
        await EnrichDtosAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PLineDto>>> GetByPackingHeaderIdAsync(long packingHeaderId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.PackingHeaderId == packingHeaderId).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PLineDto>>(items);
        await EnrichDtosAsync(dtos, cancellationToken);
        return ApiResponse<IEnumerable<PLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PLineDto>> CreateAsync(CreatePLineDto createDto, CancellationToken cancellationToken = default)
    {
        var header = await _headers.Query().Where(x => x.Id == createDto.PackingHeaderId).FirstOrDefaultAsync(cancellationToken);
        if (header == null)
        {
            var msg = _localizationService.GetLocalizedString("PHeaderNotFound");
            return ApiResponse<PLineDto>.ErrorResult(msg, msg, 404);
        }

        var package = await _packages.Query().Where(x => x.Id == createDto.PackageId).FirstOrDefaultAsync(cancellationToken);
        if (package == null)
        {
            var msg = _localizationService.GetLocalizedString("PPackageNotFound");
            return ApiResponse<PLineDto>.ErrorResult(msg, msg, 404);
        }

        if (package.PackingHeaderId != createDto.PackingHeaderId)
        {
            var msg = _localizationService.GetLocalizedString("PPackageNotBelongToPHeader");
            return ApiResponse<PLineDto>.ErrorResult(msg, msg, 400);
        }

        var entity = _mapper.Map<PLine>(createDto) ?? new PLine();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        await _lines.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (header.IsMatched && !string.IsNullOrWhiteSpace(header.SourceType))
        {
            var matchResult = await MatchPackageLineAsync(header, entity, cancellationToken);
            if (!matchResult.Success || matchResult.Data <= 0)
            {
                return ApiResponse<PLineDto>.ErrorResult(matchResult.Message, matchResult.ExceptionMessage, matchResult.StatusCode);
            }

            entity.SourceRouteId = matchResult.Data;
            _lines.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var dto = _mapper.Map<PLineDto>(entity);
        await EnrichDtoAsync(dto, cancellationToken);
        return ApiResponse<PLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineCreatedSuccessfully"));
    }

    private Task<ApiResponse<long>> MatchPackageLineAsync(PHeader header, PLine line, CancellationToken cancellationToken)
    {
        var sourceType = header.SourceType?.Trim().ToUpperInvariant();
        return sourceType switch
        {
            PHeaderSourceType.GR => _goodsReceiptMatcher.MatchPackageLineToGoodsReceiptAsync(header, line, cancellationToken),
            PHeaderSourceType.WT => _warehouseTransferMatcher.MatchPackageLineToWarehouseTransferAsync(header, line, cancellationToken),
            PHeaderSourceType.WO => _warehouseOutboundMatcher.MatchPackageLineToWarehouseOutboundAsync(header, line, cancellationToken),
            PHeaderSourceType.WI => _warehouseInboundMatcher.MatchPackageLineToWarehouseInboundAsync(header, line, cancellationToken),
            PHeaderSourceType.SH => _shippingMatcher.MatchPackageLineToShippingAsync(header, line, cancellationToken),
            PHeaderSourceType.PR => _productionMatcher.MatchPackageLineToProductionAsync(header, line, cancellationToken),
            PHeaderSourceType.PT => _productionTransferMatcher.MatchPackageLineToProductionTransferAsync(header, line, cancellationToken),
            PHeaderSourceType.SIT => _subcontractingIssueMatcher.MatchPackageLineToSubcontractingIssueAsync(header, line, cancellationToken),
            PHeaderSourceType.SRT => _subcontractingReceiptMatcher.MatchPackageLineToSubcontractingReceiptAsync(header, line, cancellationToken),
            _ => Task.FromResult(ApiResponse<long>.ErrorResult(
                _localizationService.GetLocalizedString("UnsupportedMappingSourceType"),
                _localizationService.GetLocalizedString("UnsupportedMappingSourceType"),
                400))
        };
    }

    public async Task<ApiResponse<PLineDto>> UpdateAsync(long id, UpdatePLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PLineNotFound");
            return ApiResponse<PLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PLineDto>(entity);
        await EnrichDtoAsync(dto, cancellationToken);
        return ApiResponse<PLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PLineDeletedSuccessfully"));
    }

    private async Task EnrichDtoAsync(PLineDto dto, CancellationToken cancellationToken)
    {
        await EnrichDtosAsync(new List<PLineDto> { dto }, cancellationToken);
    }

    private async Task EnrichDtosAsync(IList<PLineDto> dtos, CancellationToken cancellationToken)
    {
        if (dtos.Count == 0)
        {
            return;
        }

        var ids = dtos
            .Where(x => x.YapKodId.HasValue)
            .Select(x => x.YapKodId!.Value)
            .Distinct()
            .ToList();

        var codes = dtos
            .Where(x => !x.YapKodId.HasValue && !string.IsNullOrWhiteSpace(x.YapKod))
            .Select(x => x.YapKod!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (ids.Count == 0 && codes.Count == 0)
        {
            return;
        }

        var yapKodlar = await _yapKodlar.Query()
            .Where(x => (ids.Count > 0 && ids.Contains(x.Id)) || (codes.Count > 0 && codes.Contains(x.YapKodCode)))
            .ToListAsync(cancellationToken);

        var byId = yapKodlar.ToDictionary(x => x.Id);
        var byCode = yapKodlar
            .GroupBy(x => x.YapKodCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

        foreach (var dto in dtos)
        {
            YapKodEntity? matched = null;

            if (dto.YapKodId.HasValue)
            {
                byId.TryGetValue(dto.YapKodId.Value, out matched);
            }

            if (matched == null && !string.IsNullOrWhiteSpace(dto.YapKod))
            {
                byCode.TryGetValue(dto.YapKod, out matched);
            }

            if (matched == null)
            {
                continue;
            }

            dto.YapKodId ??= matched.Id;
            dto.YapKod = matched.YapKodCode;
            dto.YapAcik = matched.YapAcik;
        }
    }
}
