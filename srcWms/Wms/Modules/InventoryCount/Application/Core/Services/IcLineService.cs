using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcLineService : IIcLineService
{
    private readonly IRepository<IcLine> _lines;
    private readonly IRepository<IcHeader> _headers;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly IEntityReferenceResolver _entityReferenceResolver;

    public IcLineService(IRepository<IcLine> lines, IRepository<IcHeader> headers, IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IEntityReferenceResolver entityReferenceResolver)
    {
        _lines = lines;
        _headers = headers;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _entityReferenceResolver = entityReferenceResolver;
    }

    public async Task<ApiResponse<IEnumerable<IcLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
    {
        var items = await _lines.Query().Where(x => x.HeaderId == headerId && !x.IsDeleted).OrderBy(x => x.SequenceNo).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<IcLineDto>>.SuccessResult(_mapper.Map<List<IcLineDto>>(items), _localizationService.GetLocalizedString("IcLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("IcLineNotFound");
            return ApiResponse<IcLineDto>.ErrorResult(msg, msg, 404);
        }

        return ApiResponse<IcLineDto>.SuccessResult(_mapper.Map<IcLineDto>(entity), _localizationService.GetLocalizedString("IcLineRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcLineDto>> CreateAsync(CreateIcLineDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<IcLine>(createDto) ?? new IcLine();
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.CreatedDate = DateTimeProvider.Now;
        entity.IsDeleted = false;
        entity.CountStatus = "Pending";
        await _lines.AddAsync(entity, cancellationToken);

        var header = await _headers.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == createDto.HeaderId && !x.IsDeleted, cancellationToken);
        if (header != null)
        {
            header.LineCount = await _lines.Query().CountAsync(x => x.HeaderId == createDto.HeaderId && !x.IsDeleted, cancellationToken) + 1;
            header.UpdatedDate = DateTimeProvider.Now;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcLineDto>.SuccessResult(_mapper.Map<IcLineDto>(entity), _localizationService.GetLocalizedString("IcLineCreatedSuccessfully"));
    }

    public async Task<ApiResponse<IcLineDto>> UpdateAsync(long id, UpdateIcLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("IcLineNotFound");
            return ApiResponse<IcLineDto>.ErrorResult(msg, msg, 404);
        }

        _mapper.Map(updateDto, entity);
        await _entityReferenceResolver.ResolveAsync(entity, cancellationToken);
        entity.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcLineDto>.SuccessResult(_mapper.Map<IcLineDto>(entity), _localizationService.GetLocalizedString("IcLineUpdatedSuccessfully"));
    }

    public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _lines.Query().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (entity == null)
        {
            var msg = _localizationService.GetLocalizedString("IcLineNotFound");
            return ApiResponse<bool>.ErrorResult(msg, msg, 404);
        }

        await _lines.SoftDelete(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcLineDeletedSuccessfully"));
    }
}
