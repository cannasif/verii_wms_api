using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcScopeService : IIcScopeService
{
    private readonly IRepository<IcScope> _scopes;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public IcScopeService(IRepository<IcScope> scopes, IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IEntityReferenceResolver entityReferenceResolver)
    {
        _scopes = scopes;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<IcScopeDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _scopes.Query().Where(x => x.HeaderId == headerId && !x.IsDeleted).OrderBy(x => x.SequenceNo).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<IcScopeDto>>.SuccessResult(_mapper.Map<List<IcScopeDto>>(items), _localizationService.GetLocalizedString("IcScopeRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcScopeDto>> CreateAsync(CreateIcScopeDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<IcScope>(createDto) ?? new IcScope();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;
        await _scopes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcScopeDto>.SuccessResult(_mapper.Map<IcScopeDto>(entity), _localizationService.GetLocalizedString("IcScopeCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IcScopeDto>> UpdateAsync(long id, UpdateIcScopeDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _scopes.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("IcScopeNotFound");
            return ApiResponse<IcScopeDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _scopes.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcScopeDto>.SuccessResult(_mapper.Map<IcScopeDto>(entity), _localizationService.GetLocalizedString("IcScopeUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _scopes.Query().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("IcScopeNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _scopes.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcScopeDeletedSuccessfully"));
    }
}
