using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Package.Dtos;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Package.Enums;

namespace Wms.Application.Package.Services;

public sealed class PPackageService : IPPackageService
{
    private readonly IRepository<PPackage> _packages;
    private readonly IRepository<PHeader> _headers;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;

    public PPackageService(
        IRepository<PPackage> packages,
        IRepository<PHeader> headers,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
    {
        _packages = packages;
        _headers = headers;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
    }

    public async Task<ApiResponse<IEnumerable<PPackageDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _packages.Query().ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PPackageDto>>.SuccessResult(_mapper.Map<List<PPackageDto>>(items), _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PagedResponse<PPackageDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;
        var query = _packages.Query()
            .ApplyFilters(request.Filters, request.FilterLogic)
            .ApplySorting(request.SortBy ?? "Id", string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.ApplyPagination(pageNumber, pageSize).ToListAsync(cancellationToken);
        var dtos = _mapper.Map<List<PPackageDto>>(items);
        return ApiResponse<PagedResponse<PPackageDto>>.SuccessResult(new PagedResponse<PPackageDto>(dtos, total, pageNumber, pageSize), _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PPackageDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _packages.Query().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PPackageNotFound");
            return ApiResponse<PPackageDto?>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<PPackageDto?>.SuccessResult(_mapper.Map<PPackageDto>(entity), _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IEnumerable<PPackageDto>>> GetByPackingHeaderIdAsync(long packingHeaderId, CancellationToken cancellationToken = default)
    {
        var items = await _packages.Query().Where(x => x.PackingHeaderId == packingHeaderId).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<PPackageDto>>.SuccessResult(_mapper.Map<List<PPackageDto>>(items), _localizationService.GetLocalizedString("PPackageRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<PPackageDto>> CreateAsync(CreatePPackageDto createDto, CancellationToken cancellationToken = default)
    {
        var header = await _headers.Query().Where(x => x.Id == createDto.PackingHeaderId).FirstOrDefaultAsync(cancellationToken);
        if (header == null || header.IsDeleted)
        {
            var msg = _localizationService.GetLocalizedString("PHeaderNotFound");
            return ApiResponse<PPackageDto>.ErrorResult(msg, msg, 404);
        }

        var entity = _mapper.Map<PPackage>(createDto) ?? new PPackage();
        if (string.IsNullOrWhiteSpace(entity.PackageType)) entity.PackageType = PPackageType.Box;
        if (string.IsNullOrWhiteSpace(entity.Status)) entity.Status = PPackageStatus.Open;

        await _packages.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PPackageDto>.SuccessResult(_mapper.Map<PPackageDto>(entity), _localizationService.GetLocalizedString("PPackageCreatedSuccessfully"));
    }

    public async Task<ApiResponse<PPackageDto>> UpdateAsync(long id, UpdatePPackageDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _packages.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PPackageNotFound");
            return ApiResponse<PPackageDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        _packages.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<PPackageDto>.SuccessResult(_mapper.Map<PPackageDto>(entity), _localizationService.GetLocalizedString("PPackageUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _packages.Query(tracking: true).Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("PPackageNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _packages.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("PPackageDeletedSuccessfully"));
    }
}
