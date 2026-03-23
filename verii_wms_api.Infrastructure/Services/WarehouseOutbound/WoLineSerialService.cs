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
    public class WoLineSerialService : IWoLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WoLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.WoLineSerials.Query().ToListAsync();
                var dtos = _mapper.Map<IEnumerable<WoLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WoLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WoLineSerialDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<WoLineSerialDto>>.ErrorResult(
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

                var result = new PagedResponse<WoLineSerialDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WoLineSerialDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WoLineSerialDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<WoLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoLineSerials.Query().FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialNotFound"), _localizationService.GetLocalizedString("WoLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<WoLineSerialDto>(entity);
                return ApiResponse<WoLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WoLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.WoLineSerials.Query().Where(x => x.LineId == lineId).ToListAsync();
                var dtos = _mapper.Map<IEnumerable<WoLineSerialDto>>(items);
                return ApiResponse<IEnumerable<WoLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WoLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WoLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoLineSerialDto>> CreateAsync(CreateWoLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.WoLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 400);
                }
                var entity = _mapper.Map<WoLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.WoLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoLineSerialDto>(entity);
                return ApiResponse<WoLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WoLineSerialDto>> UpdateAsync(long id, UpdateWoLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.WoLineSerials.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialNotFound"), _localizationService.GetLocalizedString("WoLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.WoLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineNotFound"), _localizationService.GetLocalizedString("WoLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.WoLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WoLineSerialDto>(entity);
                return ApiResponse<WoLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WoLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WoLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WoLineSerials.Query(tracking: true).FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialNotFound"), _localizationService.GetLocalizedString("WoLineSerialNotFound"), 404);
                }
                var lineEntity = await _unitOfWork.WoLines.Query().FirstOrDefaultAsync(x => x.Id == entity.LineId);

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.WoRoutes
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
                            var msg = _localizationService.GetLocalizedString("WoLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.WoLineSerials
                    .Query()
                    .Where(ls => ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.WoRoutes
                    .Query()
                    .Where(r => r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("WoLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var currentSerialCount = await _unitOfWork.WoLineSerials
                    .Query()
                    .CountAsync(ls => ls.LineId == entity.LineId);
                var remainingSerialCount = currentSerialCount - 1;

                var hasImportLines = await _unitOfWork.WoImportLines
                    .Query()
                    .AnyAsync(il => il.LineId == entity.LineId);
                var lineWillBeDeleted = remainingSerialCount == 0 && !hasImportLines;

                var headerWillBeDeleted = false;
                var headerIdToDelete = 0L;
                if (lineWillBeDeleted && lineEntity != null)
                {
                    var headerId = lineEntity.HeaderId;
                    var currentLinesUnderHeader = await _unitOfWork.WoLines
                        .Query()
                        .CountAsync(l => l.HeaderId == headerId);
                    var remainingLinesUnderHeader = currentLinesUnderHeader - 1;
                    if (remainingLinesUnderHeader == 0)
                    {
                        var hasHeaderImportLines = await _unitOfWork.WoImportLines
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
                    await _unitOfWork.WoLineSerials.SoftDelete(id);

                    if (lineWillBeDeleted)
                    {
                        await _unitOfWork.WoLines.SoftDelete(entity.LineId);
                        if (headerWillBeDeleted && headerIdToDelete != 0)
                        {
                            await _unitOfWork.WoHeaders.SoftDelete(headerIdToDelete);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    var msgKey = lineWillBeDeleted ? "WoLineSerialDeletedAndLineDeleted" : "WoLineSerialDeletedSuccessfully";
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
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WoLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
