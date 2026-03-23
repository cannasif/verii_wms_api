using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class WiLineSerialService : IWiLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WiLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.WiLineSerials.Query().ToListAsync();
                var dtos = _mapper.Map<IEnumerable<WiLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WiLineSerialDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<WiLineSerialDto>>.ErrorResult(
                        allResult.Message,
                        allResult.ExceptionMessage,
                        allResult.StatusCode);
                }

                var query = allResult.Data.AsQueryable();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = query.Count();
                var items = query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToList();

                var result = new PagedResponse<WiLineSerialDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WiLineSerialDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WiLineSerialDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<WiLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiLineSerials.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.WiLineSerials.Query().Where(x => x.LineId == lineId).ToListAsync();
                var dtos = _mapper.Map<IEnumerable<WiLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WiLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> CreateAsync(CreateWiLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.WiLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 400);
                }
                var entity = _mapper.Map<WiLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.WiLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiLineSerialDto>> UpdateAsync(long id, UpdateWiLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WiLineSerials.Query(tracking: true)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.WiLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineNotFound"), _localizationService.GetLocalizedString("WiLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.WiLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiLineSerialDto>(entity);
                return ApiResponse<WiLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiLineSerials.Query(tracking: true)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialNotFound"), _localizationService.GetLocalizedString("WiLineSerialNotFound"), 404);
                }
                var lineEntity = await _unitOfWork.WiLines.Query()
                    .Where(x => x.Id == entity.LineId)
                    .FirstOrDefaultAsync();

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.WiRoutes
                            .Query()
                            .AnyAsync(r => r.ImportLine.LineId == entity.LineId
                                           && (
                                               (!string.IsNullOrWhiteSpace(s1) && (r.SerialNo ?? "").Trim() == s1) ||
                                               (!string.IsNullOrWhiteSpace(s2) && (r.SerialNo2 ?? "").Trim() == s2) ||
                                               (!string.IsNullOrWhiteSpace(s3) && (r.SerialNo3 ?? "").Trim() == s3) ||
                                               (!string.IsNullOrWhiteSpace(s4) && (r.SerialNo4 ?? "").Trim() == s4)
                                           ));
                        if (serialExistsInRoutes)
                        {
                            var msg = _localizationService.GetLocalizedString("WiLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.WiLineSerials
                    .Query()
                    .Where(ls => ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.WiRoutes
                    .Query()
                    .Where(r => r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("WiLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var currentSerialCount = await _unitOfWork.WiLineSerials
                    .Query()
                    .CountAsync(ls => ls.LineId == entity.LineId);
                var remainingSerialCount = currentSerialCount - 1;

                var hasImportLines = await _unitOfWork.WiImportLines
                    .Query()
                    .AnyAsync(il => il.LineId == entity.LineId);
                var lineWillBeDeleted = remainingSerialCount == 0 && !hasImportLines;

                var headerWillBeDeleted = false;
                var headerIdToDelete = 0L;
                if (lineWillBeDeleted && lineEntity != null)
                {
                    var headerId = lineEntity.HeaderId;
                    var currentLinesUnderHeader = await _unitOfWork.WiLines
                        .Query()
                        .CountAsync(l => l.HeaderId == headerId);
                    var remainingLinesUnderHeader = currentLinesUnderHeader - 1;
                    if (remainingLinesUnderHeader == 0)
                    {
                        var hasHeaderImportLines = await _unitOfWork.WiImportLines
                            .Query()
                            .AnyAsync(il => il.HeaderId == headerId);
                        if (!hasHeaderImportLines)
                        {
                            headerWillBeDeleted = true;
                            headerIdToDelete = headerId;
                        }
                    }
                }

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.WiLineSerials.SoftDelete(id);

                    if (lineWillBeDeleted)
                    {
                        await _unitOfWork.WiLines.SoftDelete(entity.LineId);
                        if (headerWillBeDeleted && headerIdToDelete != 0)
                        {
                            await _unitOfWork.WiHeaders.SoftDelete(headerIdToDelete);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    var msgKey = lineWillBeDeleted ? "WiLineSerialDeletedAndLineDeleted" : "WiLineSerialDeletedSuccessfully";
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString(msgKey));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
