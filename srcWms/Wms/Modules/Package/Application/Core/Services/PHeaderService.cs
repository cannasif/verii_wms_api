using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Application.Package.Dtos;
using Wms.Application.Production.Dtos;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Application.Shipping.Dtos;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;
using Wms.Domain.Entities.Production;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Domain.Entities.Shipping;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;
using Wms.Domain.Entities.WarehouseInbound;
using Wms.Domain.Entities.WarehouseOutbound;
using Wms.Domain.Entities.WarehouseTransfer;

namespace Wms.Application.Package.Services;

public sealed class PHeaderService : IPHeaderService
{
    private readonly IRepository<PHeader> _headers;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly IRepository<PLine> _packageLines;
    private readonly IRepository<GrHeader> _grHeaders;
    private readonly IPackageGoodsReceiptMatcher _goodsReceiptMatcher;
    private readonly IRepository<GrRoute> _grRoutes;
    private readonly IRepository<WtHeader> _wtHeaders;
    private readonly IRepository<WtRoute> _wtRoutes;
    private readonly IPackageWarehouseTransferMatcher _warehouseTransferMatcher;
    private readonly IRepository<WoHeader> _woHeaders;
    private readonly IRepository<WoRoute> _woRoutes;
    private readonly IPackageWarehouseOutboundMatcher _warehouseOutboundMatcher;
    private readonly IRepository<WiHeader> _wiHeaders;
    private readonly IRepository<WiRoute> _wiRoutes;
    private readonly IPackageWarehouseInboundMatcher _warehouseInboundMatcher;
    private readonly IRepository<ShHeader> _shHeaders;
    private readonly IRepository<ShRoute> _shRoutes;
    private readonly IPackageShippingMatcher _shippingMatcher;
    private readonly IRepository<PrHeader> _prHeaders;
    private readonly IRepository<PrRoute> _prRoutes;
    private readonly IPackageProductionMatcher _productionMatcher;
    private readonly IRepository<PtHeader> _ptHeaders;
    private readonly IRepository<PtRoute> _ptRoutes;
    private readonly IPackageProductionTransferMatcher _productionTransferMatcher;
    private readonly IRepository<SitHeader> _sitHeaders;
    private readonly IRepository<SitRoute> _sitRoutes;
    private readonly IPackageSubcontractingIssueMatcher _subcontractingIssueMatcher;
    private readonly IRepository<SrtHeader> _srtHeaders;
    private readonly IRepository<SrtRoute> _srtRoutes;
    private readonly IPackageSubcontractingReceiptMatcher _subcontractingReceiptMatcher;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public PHeaderService(
        IRepository<PHeader> headers,
        IRepository<PLine> packageLines,
        IRepository<GrHeader> grHeaders,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        IPackageGoodsReceiptMatcher goodsReceiptMatcher,
        IRepository<GrRoute> grRoutes,
        IRepository<WtHeader> wtHeaders,
        IRepository<WtRoute> wtRoutes,
        IPackageWarehouseTransferMatcher warehouseTransferMatcher,
        IRepository<WoHeader> woHeaders,
        IRepository<WoRoute> woRoutes,
        IPackageWarehouseOutboundMatcher warehouseOutboundMatcher,
        IRepository<WiHeader> wiHeaders,
        IRepository<WiRoute> wiRoutes,
        IPackageWarehouseInboundMatcher warehouseInboundMatcher,
        IRepository<ShHeader> shHeaders,
        IRepository<ShRoute> shRoutes,
        IPackageShippingMatcher shippingMatcher,
        IRepository<PrHeader> prHeaders,
        IRepository<PrRoute> prRoutes,
        IPackageProductionMatcher productionMatcher,
        IRepository<PtHeader> ptHeaders,
        IRepository<PtRoute> ptRoutes,
        IPackageProductionTransferMatcher productionTransferMatcher,
        IRepository<SitHeader> sitHeaders,
        IRepository<SitRoute> sitRoutes,
        IPackageSubcontractingIssueMatcher subcontractingIssueMatcher,
        IRepository<SrtHeader> srtHeaders,
        IRepository<SrtRoute> srtRoutes,
        IPackageSubcontractingReceiptMatcher subcontractingReceiptMatcher,
        IEntityReferenceResolver entityReferenceResolver)
    {
        _headers = headers;
        _packageLines = packageLines;
        _grHeaders = grHeaders;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _goodsReceiptMatcher = goodsReceiptMatcher;
        _grRoutes = grRoutes;
        _wtHeaders = wtHeaders;
        _wtRoutes = wtRoutes;
        _warehouseTransferMatcher = warehouseTransferMatcher;
        _woHeaders = woHeaders;
        _woRoutes = woRoutes;
        _warehouseOutboundMatcher = warehouseOutboundMatcher;
        _wiHeaders = wiHeaders;
        _wiRoutes = wiRoutes;
        _warehouseInboundMatcher = warehouseInboundMatcher;
        _shHeaders = shHeaders;
        _shRoutes = shRoutes;
        _shippingMatcher = shippingMatcher;
        _prHeaders = prHeaders;
        _prRoutes = prRoutes;
        _productionMatcher = productionMatcher;
        _ptHeaders = ptHeaders;
        _ptRoutes = ptRoutes;
        _productionTransferMatcher = productionTransferMatcher;
        _sitHeaders = sitHeaders;
        _sitRoutes = sitRoutes;
        _subcontractingIssueMatcher = subcontractingIssueMatcher;
        _srtHeaders = srtHeaders;
        _srtRoutes = srtRoutes;
        _subcontractingReceiptMatcher = subcontractingReceiptMatcher;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<PHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _headers.Query().ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PHeaderDto>>(items);
        return ApiResponse<IEnumerable<PHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var query = _headers.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(pageNumber, pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PHeaderDto>>(items);
        return ApiResponse<PagedResponse<PHeaderDto>>.SuccessResult(new PagedResponse<PHeaderDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PHeaderDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PHeaderNotFound");
            return ApiResponse<PHeaderDto?>.ErrorResult(msg, msg, 404);
        }

        var dto = _mapper.Map<PHeaderDto>(entity);
        return ApiResponse<PHeaderDto?>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PHeaderDto>> CreateAsync(CreatePHeaderDto createDto, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(createDto.PackingNo))
        {
            var exists = await _headers.Query().AnyAsync(x => x.PackingNo == createDto.PackingNo, cancellationToken);
            if (exists)
            {
                var msg = _localizationService.GetLocalizedString("PHeaderPackingNoAlreadyExists");
                return ApiResponse<PHeaderDto>.ErrorResult(msg, msg, 400);
            }
        }

        var entity = _mapper.Map<PHeader>(createDto) ?? new PHeader();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        if (string.IsNullOrWhiteSpace(entity.Status))
        {
            entity.Status = PHeaderStatus.Draft;
        }

        await _headers.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PHeaderDto>(entity);
        return ApiResponse<PHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PHeaderDto>> UpdateAsync(long id, UpdatePHeaderDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PHeaderNotFound");
            return ApiResponse<PHeaderDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        _headers.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<PHeaderDto>(entity);
        return ApiResponse<PHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PHeaderUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _headers.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        if (entity.IsMatched)
        {
            var msg = _localizationService.GetLocalizedString("PHeaderCannotDeleteWhenMatched");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        await _headers.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PHeaderDeletedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> MatchPlinesWithMatchedStatus(long pHeaderId, bool isMatched, CancellationToken cancellationToken = default)
    {
        var pHeader = await _headers.Query(tracking: true).Where(x => x.Id == pHeaderId).FirstOrDefaultAsync(cancellationToken);
        if (pHeader == null)
        {
            var msg = _localizationService.GetLocalizedString("PHeaderNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        if (string.IsNullOrWhiteSpace(pHeader.SourceType))
        {
            var msg = _localizationService.GetLocalizedString("InvalidSourceType");
            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
        }

        var packageLines = await _packageLines.Query(tracking: true)
            .Where(pl => pl.PackingHeaderId == pHeaderId)
            .ToListAsync(cancellationToken);
        if (packageLines.Count == 0)
        {
            var msg = _localizationService.GetLocalizedString("PLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (isMatched)
            {
                foreach (var packageLine in packageLines)
                {
                    if (packageLine.SourceRouteId.HasValue)
                    {
                        continue;
                    }

                    var result = await MatchPackageLineAsync(pHeader, packageLine, cancellationToken);
                    if (!result.Success || result.Data <= 0)
                    {
                        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ApiResponse<bool>.ErrorResult(result.Message, result.ExceptionMessage, result.StatusCode);
                    }

                    packageLine.SourceRouteId = result.Data;
                    _packageLines.Update(packageLine);
                }
            }
            else
            {
                var sourceHeaderStatus = await GetSourceHeaderStateAsync(pHeader, cancellationToken);
                if (!sourceHeaderStatus.IsValid)
                {
                    var msg = _localizationService.GetLocalizedString("MatchedSourceHeaderMustBeActiveAndIncomplete");
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                foreach (var packageLine in packageLines)
                {
                    if (packageLine.SourceRouteId.HasValue)
                    {
                        await SoftDeleteMappedRouteAsync(pHeader.SourceType, packageLine.SourceRouteId.Value, cancellationToken);
                    }

                    packageLine.SourceRouteId = null;
                    _packageLines.Update(packageLine);
                }
            }

            pHeader.IsMatched = isMatched;
            _headers.Update(pHeader);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PHeaderMatchedSuccessfully"));
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ApiResponse<IEnumerable<object>>> GetAvailableHeadersForMappingAsync(string sourceType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sourceType))
        {
            var msg = _localizationService.GetLocalizedString("InvalidSourceType");
            return ApiResponse<IEnumerable<object>>.ErrorResult(msg, msg, 400);
        }

        var normalizedSourceType = sourceType.Trim().ToUpperInvariant();
        var mappedHeaderIds = await _headers.Query()
            .Where(ph => ph.SourceType == normalizedSourceType && ph.SourceHeaderId.HasValue)
            .Select(ph => ph.SourceHeaderId!.Value)
            .ToListAsync(cancellationToken);

        if (normalizedSourceType == PHeaderSourceType.GR)
        {
            var headers = await _grHeaders.Query()
                .Where(h => !mappedHeaderIds.Contains(h.Id))
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<GrHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }

        if (normalizedSourceType == PHeaderSourceType.WT)
        {
            var headers = await _wtHeaders.Query()
                .Where(h => !mappedHeaderIds.Contains(h.Id))
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<WtHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }

        if (normalizedSourceType == PHeaderSourceType.WO)
        {
            var headers = await _woHeaders.Query()
                .Where(h => !mappedHeaderIds.Contains(h.Id))
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<WoHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }

        if (normalizedSourceType == PHeaderSourceType.WI)
        {
            var headers = await _wiHeaders.Query()
                .Where(h => !mappedHeaderIds.Contains(h.Id))
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<WiHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }

        if (normalizedSourceType == PHeaderSourceType.SH)
        {
            var headers = await _shHeaders.Query()
                .Where(h => !mappedHeaderIds.Contains(h.Id))
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<ShHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }
        if (normalizedSourceType == PHeaderSourceType.PR)
        {
            var headers = await _prHeaders.Query().Where(h => !mappedHeaderIds.Contains(h.Id)).ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<PrHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }
        if (normalizedSourceType == PHeaderSourceType.PT)
        {
            var headers = await _ptHeaders.Query().Where(h => !mappedHeaderIds.Contains(h.Id)).ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<PtHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }
        if (normalizedSourceType == PHeaderSourceType.SIT)
        {
            var headers = await _sitHeaders.Query().Where(h => !mappedHeaderIds.Contains(h.Id)).ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<SitHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }
        if (normalizedSourceType == PHeaderSourceType.SRT)
        {
            var headers = await _srtHeaders.Query().Where(h => !mappedHeaderIds.Contains(h.Id)).ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<SrtHeaderDto>>(headers);
            return ApiResponse<IEnumerable<object>>.SuccessResult(dtos.Cast<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
        }

        return ApiResponse<IEnumerable<object>>.SuccessResult(Array.Empty<object>(), _localizationService.GetLocalizedString("AvailableHeadersRetrievedSuccessfully"));
    }

    private Task<ApiResponse<long>> MatchPackageLineAsync(PHeader header, PLine packageLine, CancellationToken cancellationToken)
    {
        var sourceType = header.SourceType?.Trim().ToUpperInvariant();
        return sourceType switch
        {
            PHeaderSourceType.GR => _goodsReceiptMatcher.MatchPackageLineToGoodsReceiptAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.WT => _warehouseTransferMatcher.MatchPackageLineToWarehouseTransferAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.WO => _warehouseOutboundMatcher.MatchPackageLineToWarehouseOutboundAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.WI => _warehouseInboundMatcher.MatchPackageLineToWarehouseInboundAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.SH => _shippingMatcher.MatchPackageLineToShippingAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.PR => _productionMatcher.MatchPackageLineToProductionAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.PT => _productionTransferMatcher.MatchPackageLineToProductionTransferAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.SIT => _subcontractingIssueMatcher.MatchPackageLineToSubcontractingIssueAsync(header, packageLine, cancellationToken),
            PHeaderSourceType.SRT => _subcontractingReceiptMatcher.MatchPackageLineToSubcontractingReceiptAsync(header, packageLine, cancellationToken),
            _ => Task.FromResult(ApiResponse<long>.ErrorResult(
                _localizationService.GetLocalizedString("UnsupportedMappingSourceType"),
                _localizationService.GetLocalizedString("UnsupportedMappingSourceType"),
                400))
        };
    }

    private async Task<(bool IsValid, long? Id)> GetSourceHeaderStateAsync(PHeader header, CancellationToken cancellationToken)
    {
        var sourceType = header.SourceType?.Trim().ToUpperInvariant();
        if (!header.SourceHeaderId.HasValue)
        {
            return (false, null);
        }

        if (sourceType == PHeaderSourceType.GR)
        {
            var sourceHeader = await _grHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }

        if (sourceType == PHeaderSourceType.WT)
        {
            var sourceHeader = await _wtHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }

        if (sourceType == PHeaderSourceType.WO)
        {
            var sourceHeader = await _woHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }

        if (sourceType == PHeaderSourceType.WI)
        {
            var sourceHeader = await _wiHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }

        if (sourceType == PHeaderSourceType.SH)
        {
            var sourceHeader = await _shHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }
        if (sourceType == PHeaderSourceType.PR)
        {
            var sourceHeader = await _prHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }
        if (sourceType == PHeaderSourceType.PT)
        {
            var sourceHeader = await _ptHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }
        if (sourceType == PHeaderSourceType.SIT)
        {
            var sourceHeader = await _sitHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }
        if (sourceType == PHeaderSourceType.SRT)
        {
            var sourceHeader = await _srtHeaders.Query(tracking: true).Where(x => x.Id == header.SourceHeaderId.Value).FirstOrDefaultAsync(cancellationToken);
            return (sourceHeader != null && !sourceHeader.IsDeleted && !sourceHeader.IsCompleted, sourceHeader?.Id);
        }

        return (false, null);
    }

    private Task SoftDeleteMappedRouteAsync(string? sourceType, long routeId, CancellationToken cancellationToken)
    {
        var normalizedSourceType = sourceType?.Trim().ToUpperInvariant();
        return normalizedSourceType switch
        {
            PHeaderSourceType.GR => _grRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.WT => _wtRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.WO => _woRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.WI => _wiRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.SH => _shRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.PR => _prRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.PT => _ptRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.SIT => _sitRoutes.SoftDelete(routeId, cancellationToken),
            PHeaderSourceType.SRT => _srtRoutes.SoftDelete(routeId, cancellationToken),
            _ => Task.CompletedTask
        };
    }
}
