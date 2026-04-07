using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Domain.Common;
using Wms.Domain.Entities.InventoryCount;

namespace Wms.Application.InventoryCount.Services;

public sealed class IcCountEntryService : IIcCountEntryService
{
    private readonly IRepository<IcCountEntry> _entries;
    private readonly IRepository<IcLine> _lines;
    private readonly IRepository<IcHeader> _headers;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public IcCountEntryService(
        IRepository<IcCountEntry> entries,
        IRepository<IcLine> lines,
        IRepository<IcHeader> headers,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILocalizationService localizationService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _entries = entries;
        _lines = lines;
        _headers = headers;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizationService = localizationService;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<ApiResponse<IEnumerable<IcCountEntryDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default)
    {
        var items = await _entries.Query().Where(x => x.LineId == lineId && !x.IsDeleted).OrderByDescending(x => x.EnteredAt).ToListAsync(cancellationToken);
        return ApiResponse<IEnumerable<IcCountEntryDto>>.SuccessResult(_mapper.Map<List<IcCountEntryDto>>(items), _localizationService.GetLocalizedString("IcCountEntryRetrievedSuccessfully"));
    }

    public async Task<ApiResponse<IcCountEntryDto>> CreateAsync(CreateIcCountEntryDto createDto, CancellationToken cancellationToken = default)
    {
        var line = await _lines.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == createDto.LineId && !x.IsDeleted, cancellationToken);
        if (line == null)
        {
            var msg = _localizationService.GetLocalizedString("IcLineNotFound");
            return ApiResponse<IcCountEntryDto>.ErrorResult(msg, msg, 404);
        }

        var existingEntryCount = await _entries.Query().CountAsync(x => x.LineId == createDto.LineId && !x.IsDeleted, cancellationToken);
        var entity = new IcCountEntry
        {
            HeaderId = line.HeaderId,
            LineId = line.Id,
            EntryNo = existingEntryCount + 1,
            EntryType = string.IsNullOrWhiteSpace(createDto.EntryType) ? "FirstCount" : createDto.EntryType,
            EnteredQuantity = createDto.EnteredQuantity,
            WarehouseId = createDto.WarehouseId ?? line.WarehouseId,
            WarehouseCode = createDto.WarehouseCode ?? line.WarehouseCode,
            RackCode = createDto.RackCode ?? line.RackCode,
            CellCode = createDto.CellCode ?? line.CellCode,
            EnteredAt = DateTimeProvider.Now,
            EnteredByUserId = _currentUserAccessor.UserId,
            DeviceCode = createDto.DeviceCode,
            Note = createDto.Note,
            CreatedDate = DateTimeProvider.Now,
            IsDeleted = false,
        };

        await _entries.AddAsync(entity, cancellationToken);

        var allEnteredQuantity = await _entries.Query()
            .Where(x => x.LineId == line.Id && !x.IsDeleted)
            .Select(x => x.EnteredQuantity)
            .ToListAsync(cancellationToken);
        var newCountedQuantity = allEnteredQuantity.Sum() + createDto.EnteredQuantity;
        var difference = newCountedQuantity - line.ExpectedQuantity;

        line.CountedQuantity = newCountedQuantity;
        line.DifferenceQuantity = difference;
        line.EntryCount = existingEntryCount + 1;
        line.IsMatched = difference == 0;
        line.IsDifference = difference != 0;
        line.IsExtraStock = difference > 0;
        line.IsMissingStock = difference < 0;
        line.IsRecountRequired = difference != 0;
        line.CountStatus = difference == 0 ? "Counted" : "Recount";
        line.CountedByUserId = _currentUserAccessor.UserId;
        line.FirstCountedAt ??= entity.EnteredAt;
        line.LastCountedAt = entity.EnteredAt;
        line.UpdatedDate = DateTimeProvider.Now;
        _lines.Update(line);

        var header = await _headers.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == line.HeaderId && !x.IsDeleted, cancellationToken);
        if (header != null)
        {
            var headerLines = await _lines.Query().Where(x => x.HeaderId == header.Id && !x.IsDeleted).ToListAsync(cancellationToken);
            header.LineCount = headerLines.Count;
            header.CountedLineCount = headerLines.Count(x => x.EntryCount > 0 || x.CountedQuantity.HasValue);
            header.DifferenceLineCount = headerLines.Count(x => x.IsDifference);
            header.RecountRequiredLineCount = headerLines.Count(x => x.IsRecountRequired);
            header.RequiresRecount = (header.RecountRequiredLineCount ?? 0) > 0;
            header.Status = (header.CountedLineCount ?? 0) == 0 ? header.Status : ((header.RecountRequiredLineCount ?? 0) > 0 ? "Review" : "Counting");
            header.UpdatedDate = DateTimeProvider.Now;
            _headers.Update(header);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<IcCountEntryDto>.SuccessResult(_mapper.Map<IcCountEntryDto>(entity), _localizationService.GetLocalizedString("IcCountEntryCreatedSuccessfully"));
    }
}
