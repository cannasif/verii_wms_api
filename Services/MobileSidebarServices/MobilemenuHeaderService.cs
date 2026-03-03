using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class MobilemenuHeaderService : IMobilemenuHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public MobilemenuHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<MobilemenuHeaderDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuHeaders.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<MobilemenuHeaderDto>>(entities);
                return ApiResponse<IEnumerable<MobilemenuHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilemenuHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<IEnumerable<MobilemenuHeaderDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<MobilemenuHeaderDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.MobilemenuHeaders.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<MobilemenuHeaderDto>>(entities);
                var result = new PagedResponse<MobilemenuHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<MobilemenuHeaderDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("MobilemenuHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<MobilemenuHeaderDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<MobilemenuHeaderDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.MobilemenuHeaders.GetByIdAsync(id);
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuHeaderNotFound");
                    return ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuHeaderNotFound"), 404);
                }

                var dto = _mapper.Map<MobilemenuHeaderDto>(entity);
                return ApiResponse<MobilemenuHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilemenuHeaderDto>> GetByMenuIdAsync(string menuId)
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuHeaders.FindAsync(x => x.MenuId == menuId && !x.IsDeleted);
                var entity = entities.FirstOrDefault();
                
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuHeaderNotFound");
                    return ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuHeaderNotFound"), 404);
                }

                var dto = _mapper.Map<MobilemenuHeaderDto>(entity);
                return ApiResponse<MobilemenuHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobilemenuHeaderDto>>> GetByTitleAsync(string title)
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuHeaders.FindAsync(x => x.Title.Contains(title) && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<MobilemenuHeaderDto>>(entities);
                return ApiResponse<IEnumerable<MobilemenuHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilemenuHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<IEnumerable<MobilemenuHeaderDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobilemenuHeaderDto>>> GetOpenMenusAsync()
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuHeaders.FindAsync(x => x.IsOpen && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<MobilemenuHeaderDto>>(entities);
                return ApiResponse<IEnumerable<MobilemenuHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilemenuHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<IEnumerable<MobilemenuHeaderDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilemenuHeaderDto>> CreateAsync(CreateMobilemenuHeaderDto createDto)
        {
            try
            {
                var entity = _mapper.Map<MobilemenuHeader>(createDto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.MobilemenuHeaders.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobilemenuHeaderDto>(entity);
                return ApiResponse<MobilemenuHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuHeaderCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilemenuHeaderDto>> UpdateAsync(long id, UpdateMobilemenuHeaderDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.MobilemenuHeaders.GetByIdAsync(id);
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuHeaderNotFound");
                    return ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuHeaderNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.MobilemenuHeaders.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobilemenuHeaderDto>(entity);
                return ApiResponse<MobilemenuHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuHeaderUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.MobilemenuHeaders.ExistsAsync(id);
                if (!exists)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuHeaderNotFound");
                    return ApiResponse<bool>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuHeaderNotFound"), 404);
                }

                await _unitOfWork.MobilemenuHeaders.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("MobilemenuHeaderDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuHeaderErrorOccurred");
                return ApiResponse<bool>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

    }
}
