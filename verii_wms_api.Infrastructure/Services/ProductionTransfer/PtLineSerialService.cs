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
    public class PtLineSerialService : IPtLineSerialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PtLineSerialService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetAllAsync()
        {
            try
            {
                var items = await _unitOfWork.PtLineSerials.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PtLineSerialDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<PtLineSerialDto>>.ErrorResult(
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

                var result = new PagedResponse<PtLineSerialDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<PtLineSerialDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PtLineSerialDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<PtLineSerialDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtLineSerials.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialNotFound"), _localizationService.GetLocalizedString("PtLineSerialNotFound"), 404);
                }
                var dto = _mapper.Map<PtLineSerialDto>(entity);
                return ApiResponse<PtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetByLineIdAsync(long lineId)
        {
            try
            {
                var items = await _unitOfWork.PtLineSerials.FindAsync(x => x.LineId == lineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<PtLineSerialDto>>(items);
                return ApiResponse<IEnumerable<PtLineSerialDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("PtLineSerialRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PtLineSerialDto>>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineSerialDto>> CreateAsync(CreatePtLineSerialDto createDto)
        {
            try
            {
                var lineExists = await _unitOfWork.PtLines.ExistsAsync(createDto.LineId);
                if (!lineExists)
                {
                    return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 400);
                }
                var entity = _mapper.Map<PtLineSerial>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;
                await _unitOfWork.PtLineSerials.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineSerialDto>(entity);
                return ApiResponse<PtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineSerialCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PtLineSerialDto>> UpdateAsync(long id, UpdatePtLineSerialDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PtLineSerials.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialNotFound"), _localizationService.GetLocalizedString("PtLineSerialNotFound"), 404);
                }
                if (updateDto.LineId.HasValue)
                {
                    var lineExists = await _unitOfWork.PtLines.ExistsAsync(updateDto.LineId.Value);
                    if (!lineExists)
                    {
                        return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineNotFound"), _localizationService.GetLocalizedString("PtLineNotFound"), 400);
                    }
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;
                _unitOfWork.PtLineSerials.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<PtLineSerialDto>(entity);
                return ApiResponse<PtLineSerialDto>.SuccessResult(dto, _localizationService.GetLocalizedString("PtLineSerialUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PtLineSerialDto>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PtLineSerials.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialNotFound"), _localizationService.GetLocalizedString("PtLineSerialNotFound"), 404);
                }
                var lineEntity = await _unitOfWork.PtLines.GetByIdAsync(entity.LineId);

                {
                    var s1 = (entity.SerialNo ?? "").Trim();
                    var s2 = (entity.SerialNo2 ?? "").Trim();
                    var s3 = (entity.SerialNo3 ?? "").Trim();
                    var s4 = (entity.SerialNo4 ?? "").Trim();
                    var anyEntitySerial = !string.IsNullOrWhiteSpace(s1) || !string.IsNullOrWhiteSpace(s2) || !string.IsNullOrWhiteSpace(s3) || !string.IsNullOrWhiteSpace(s4);
                    if (anyEntitySerial)
                    {
                        var serialExistsInRoutes = await _unitOfWork.PtRoutes.Query()
                            .Where(r => !r.IsDeleted
                                           && r.ImportLine.LineId == entity.LineId
                                           && (
                                               (!string.IsNullOrWhiteSpace(s1) && (r.SerialNo ?? "").Trim() == s1) ||
                                               (!string.IsNullOrWhiteSpace(s2) && (r.SerialNo2 ?? "").Trim() == s2) ||
                                               (!string.IsNullOrWhiteSpace(s3) && (r.SerialNo3 ?? "").Trim() == s3) ||
                                               (!string.IsNullOrWhiteSpace(s4) && (r.SerialNo4 ?? "").Trim() == s4)
                                           ))
                            .AnyAsync();
                        if (serialExistsInRoutes)
                        {
                            var msg = _localizationService.GetLocalizedString("PtLineSerialRoutesExist");
                            return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                        }
                    }
                }

                var totalLineSerialQty = await _unitOfWork.PtLineSerials.Query()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                    .SumAsync(ls => ls.Quantity);

                var totalRouteQty = await _unitOfWork.PtRoutes.Query()
                    .Where(r => !r.IsDeleted && r.ImportLine.LineId == entity.LineId)
                    .SumAsync(r => r.Quantity);

                var remainingAfterDelete = totalLineSerialQty - entity.Quantity;
                if (remainingAfterDelete < totalRouteQty)
                {
                    var msg = _localizationService.GetLocalizedString("PtLineSerialInsufficientQuantityAfterDelete");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var currentSerialCount = await _unitOfWork.PtLineSerials.Query()
                    .Where(ls => !ls.IsDeleted && ls.LineId == entity.LineId)
                            .CountAsync();
                var remainingSerialCount = currentSerialCount - 1;

                var hasImportLines = await _unitOfWork.PtImportLines.Query()
                    .Where(il => !il.IsDeleted && il.LineId == entity.LineId)
                            .AnyAsync();
                var lineWillBeDeleted = remainingSerialCount == 0 && !hasImportLines;

                var headerWillBeDeleted = false;
                var headerIdToDelete = 0L;
                if (lineWillBeDeleted && lineEntity != null)
                {
                    var headerId = lineEntity.HeaderId;
                    var currentLinesUnderHeader = await _unitOfWork.PtLines.Query()
                        .Where(l => !l.IsDeleted && l.HeaderId == headerId)
                            .CountAsync();
                    var remainingLinesUnderHeader = currentLinesUnderHeader - 1;
                    if (remainingLinesUnderHeader == 0)
                    {
                        var hasHeaderImportLines = await _unitOfWork.PtImportLines.Query()
                            .Where(il => !il.IsDeleted && il.HeaderId == headerId)
                            .AnyAsync();
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
                    await _unitOfWork.PtLineSerials.SoftDelete(id);

                    if (lineWillBeDeleted)
                    {
                        await _unitOfWork.PtLines.SoftDelete(entity.LineId);
                        if (headerWillBeDeleted && headerIdToDelete != 0)
                        {
                            await _unitOfWork.PtHeaders.SoftDelete(headerIdToDelete);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    var msgKey = lineWillBeDeleted ? "PtLineSerialDeletedAndLineDeleted" : "PtLineSerialDeletedSuccessfully";
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
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("PtLineSerialErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
