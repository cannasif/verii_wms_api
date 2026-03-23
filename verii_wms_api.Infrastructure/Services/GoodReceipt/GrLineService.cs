using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrLineService : IGrLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IErpService _erpService;

        public GrLineService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IErpService erpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _erpService = erpService;
        }

        public async Task<ApiResponse<PagedResponse<GrLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.GrLines.Query();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtos = _mapper.Map<List<GrLineDto>>(items);
                var enriched = await _erpService.PopulateStockNamesAsync(dtos);
                if (!enriched.Success)
                {
                    return ApiResponse<PagedResponse<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                dtos = enriched.Data?.ToList() ?? dtos;

                var result = new PagedResponse<GrLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<GrLineDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetAllAsync()
        {
            try
            {
                var lines = await _unitOfWork.GrLines.Query().ToListAsync();
                var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

                var enriched = await _erpService.PopulateStockNamesAsync(lineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(enriched.Data ?? lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetAllError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var line = await _unitOfWork.GrLines.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (line == null)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(
                        _localizationService.GetLocalizedString("GrLineNotFound"),
                        _localizationService.GetLocalizedString("RecordNotFound"),
                        404,
                        _localizationService.GetLocalizedString("GrLineMissing"));
                }

                var lineDto = _mapper.Map<GrLineDto>(line);

                var enriched = await _erpService.PopulateStockNamesAsync(new[] { lineDto });
                if (!enriched.Success)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }
                var finalDto = enriched.Data?.FirstOrDefault() ?? lineDto;

                return ApiResponse<GrLineDto>.SuccessResult(finalDto, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetByIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.GrLines.Query().Where(x => x.HeaderId == headerId).ToListAsync();
                var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

                var enriched = await _erpService.PopulateStockNamesAsync(lineDtos);
                if (!enriched.Success)
                {
                    return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
                }

                return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(enriched.Data ?? lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetByHeaderIdError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineDto>> CreateAsync(CreateGrLineDto createDto)
        {
            try
            {
                // Header'ın var olup olmadığını kontrol et
                var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)createDto.HeaderId);
                if (!headerExists)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(
                        _localizationService.GetLocalizedString("GrHeaderNotFound"),
                        _localizationService.GetLocalizedString("HeaderNotFound"),
                        400,
                        _localizationService.GetLocalizedString("GrHeaderMissing"));
                }

                var line = _mapper.Map<GrLine>(createDto);

                await _unitOfWork.GrLines.AddAsync(line);
                await _unitOfWork.SaveChangesAsync();

                var lineDto = _mapper.Map<GrLineDto>(line);
                return ApiResponse<GrLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("GrLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineCreateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<GrLineDto>> UpdateAsync(long id, UpdateGrLineDto updateDto)
        {
            try
            {
                var existingLine = await _unitOfWork.GrLines.Query(tracking: true)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (existingLine == null)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(
                        _localizationService.GetLocalizedString("GrLineNotFound"),
                        _localizationService.GetLocalizedString("RecordNotFound"),
                        404,
                        _localizationService.GetLocalizedString("GrLineMissing"));
                }

                // Header'ın var olup olmadığını kontrol et
                var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)updateDto.HeaderId);
                if (!headerExists)
                {
                    return ApiResponse<GrLineDto>.ErrorResult(
                        _localizationService.GetLocalizedString("GrHeaderNotFound"),
                        _localizationService.GetLocalizedString("HeaderNotFound"),
                        400,
                        _localizationService.GetLocalizedString("GrHeaderMissing"));
                }

                _mapper.Map(updateDto, existingLine);

                _unitOfWork.GrLines.Update(existingLine);
                await _unitOfWork.SaveChangesAsync();

                var lineDto = _mapper.Map<GrLineDto>(existingLine);
                return ApiResponse<GrLineDto>.SuccessResult(lineDto, _localizationService.GetLocalizedString("GrLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("GrLineUpdateError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.GrLines.Query(tracking: true)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("GrLineNotFound"),
                        _localizationService.GetLocalizedString("RecordNotFound"),
                        404,
                        _localizationService.GetLocalizedString("GrLineMissing"));
                }

                var hasActiveLineSerials = await _unitOfWork.GrLineSerials
                    .Query()
                    .Where(ls => ls.LineId == id)
                            .AnyAsync();
                if (hasActiveLineSerials)
                {
                    var msg = _localizationService.GetLocalizedString("GrLineLineSerialsExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var importLines = await _unitOfWork.GrImportLines.Query().Where(x => x.LineId == id).ToListAsync();
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("GrLineImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }

                var headerId = entity.HeaderId;
                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.GrLines.SoftDelete(id);

                    var hasOtherLines = await _unitOfWork.GrLines
                        .Query()
                        .Where(l => l.HeaderId == headerId)
                            .AnyAsync();
                    var hasOtherImportLines = await _unitOfWork.GrImportLines
                        .Query()
                        .Where(il => il.HeaderId == headerId)
                            .AnyAsync();
                    if (!hasOtherLines && !hasOtherImportLines)
                    {
                        await _unitOfWork.GrHeaders.SoftDelete(headerId);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("GrLineDeleteError"), ex.Message, 500);
            }
        }

        

        // GrHeader ilişkili satırları (GrLine) headerId’ye göre getirir
        public async Task<ApiResponse<IEnumerable<GrLineDto>>> GetLinesByHeaderIdAsync(long headerId)
        {
            try
            {
                var lines = await _unitOfWork.GrLines.Query().Where(x => x.HeaderId == headerId).ToListAsync();
                var lineDtos = _mapper.Map<IEnumerable<GrLineDto>>(lines);

                return ApiResponse<IEnumerable<GrLineDto>>.SuccessResult(lineDtos, _localizationService.GetLocalizedString("GrLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<GrLineDto>>.ErrorResult(_localizationService.GetLocalizedString("GrLineGetByHeaderIdError"), ex.Message, 500);
            }
        }
        
    }
}
