using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class IcHeaderService : IIcHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public IcHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<IcHeaderDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ICHeaders.Query().ToListAsync();
                var dtos = _mapper.Map<IEnumerable<IcHeaderDto>>(entities);
                return ApiResponse<IEnumerable<IcHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<IcHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<IcHeaderDto>>.ErrorResult(
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

                var result = new PagedResponse<IcHeaderDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<IcHeaderDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<IcHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<IcHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ICHeaders.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderNotFound"), _localizationService.GetLocalizedString("IcHeaderNotFound"), 404);
                }
                var dto = _mapper.Map<IcHeaderDto>(entity);
                return ApiResponse<IcHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IcHeaderDto>> CreateAsync(CreateIcHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<IcHeader>(createDto);
                entity.CreatedDate = DateTimeProvider.Now;
                entity.IsDeleted = false;
                await _unitOfWork.ICHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcHeaderDto>(entity);
                return ApiResponse<IcHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<IcHeaderDto>> UpdateAsync(long id, UpdateIcHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.ICHeaders.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderNotFound"), _localizationService.GetLocalizedString("IcHeaderNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;
                _unitOfWork.ICHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcHeaderDto>(entity);
                return ApiResponse<IcHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.ICHeaders.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderNotFound"), _localizationService.GetLocalizedString("IcHeaderNotFound"), 404);
                }
                var importLines = await _unitOfWork.IcImportLines.Query().Where(x => x.HeaderId == id).ToListAsync();
                if (importLines.Any())
                {
                    var msg = _localizationService.GetLocalizedString("IcHeaderImportLinesExist");
                    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
                }
                await _unitOfWork.ICHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcHeaderErrorOccurred"), ex.Message, 500);
            }
        }
    }
}
