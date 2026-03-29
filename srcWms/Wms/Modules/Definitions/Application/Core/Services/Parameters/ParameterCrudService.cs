using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;
using Wms.Domain.Entities.Definitions;

namespace Wms.Application.Definitions.Services.Parameters;

/// <summary>
/// `_old` Parameter servislerinin tekrar eden CRUD/paged davranışını pragmatik tek tabana indirir.
/// </summary>
public abstract class ParameterCrudService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : BaseParameter, new()
    where TDto : BaseParameterDto
    where TCreateDto : CreateBaseParameterDto
    where TUpdateDto : UpdateBaseParameterDto
{
    private readonly IRepository<TEntity> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;

    protected ParameterCrudService(
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
    }

    protected virtual string EntityNotFoundMessageKey => "ParameterNotFound";
    protected virtual string EntityRetrievedMessageKey => "ParameterRetrievedSuccessfully";
    protected virtual string EntityCreatedMessageKey => "ParameterCreatedSuccessfully";
    protected virtual string EntityUpdatedMessageKey => "ParameterUpdatedSuccessfully";
    protected virtual string EntityDeletedMessageKey => "ParameterDeletedSuccessfully";

    public async Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return ApiResponse<IEnumerable<TDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString(EntityRetrievedMessageKey));
    }

    public async Task<ApiResponse<PagedResponse<TDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        request ??= new PagedRequest();
        if (request.PageNumber < 1) request.PageNumber = 1;
        if (request.PageSize < 1) request.PageSize = 20;

        var query = _repository.Query()
            .ApplyFilters(request.Filters, request.FilterLogic);

        var desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        query = query.ApplySorting(request.SortBy ?? "Id", desc);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<TDto>>(items);
        var paged = new PagedResponse<TDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        return ApiResponse<PagedResponse<TDto>>.SuccessResult(paged, _localizationService.GetLocalizedString(EntityRetrievedMessageKey));
    }

    public async Task<ApiResponse<TDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.Query()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString(EntityNotFoundMessageKey);
            return ApiResponse<TDto>.ErrorResult(notFound, notFound, 404);
        }

        var dto = _mapper.Map<TDto>(entity);
        return ApiResponse<TDto>.SuccessResult(dto, _localizationService.GetLocalizedString(EntityRetrievedMessageKey));
    }

    public async Task<ApiResponse<TDto>> CreateAsync(TCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(createDto);
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TDto>(entity);
        return ApiResponse<TDto>.SuccessResult(dto, _localizationService.GetLocalizedString(EntityCreatedMessageKey));
    }

    public async Task<ApiResponse<TDto>> UpdateAsync(long id, TUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.Query(tracking: true)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            var notFound = _localizationService.GetLocalizedString(EntityNotFoundMessageKey);
            return ApiResponse<TDto>.ErrorResult(notFound, notFound, 404);
        }

        _mapper.Map(updateDto, entity);
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TDto>(entity);
        return ApiResponse<TDto>.SuccessResult(dto, _localizationService.GetLocalizedString(EntityUpdatedMessageKey));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            var notFound = _localizationService.GetLocalizedString(EntityNotFoundMessageKey);
            return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
        }

        await _repository.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(EntityDeletedMessageKey));
    }
}
