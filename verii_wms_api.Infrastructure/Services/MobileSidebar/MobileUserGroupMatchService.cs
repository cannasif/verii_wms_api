using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class MobileUserGroupMatchService : IMobileUserGroupMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public MobileUserGroupMatchService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.MobileUserGroupMatches.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<MobileUserGroupMatchDto>>(entities);
                return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievalError");
                return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<MobileUserGroupMatchDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.MobileUserGroupMatches.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<MobileUserGroupMatchDto>>(entities);
                var result = new PagedResponse<MobileUserGroupMatchDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<MobileUserGroupMatchDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<MobileUserGroupMatchDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievalError"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<MobileUserGroupMatchDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.MobileUserGroupMatches.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobileUserGroupMatchNotFound");
                    return ApiResponse<MobileUserGroupMatchDto>.ErrorResult(message, message, 404);
                }

                var dto = _mapper.Map<MobileUserGroupMatchDto>(entity);
                return ApiResponse<MobileUserGroupMatchDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievalError");
                return ApiResponse<MobileUserGroupMatchDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetByUserIdAsync(int userId)
        {
            try
            {
                var entities = await _unitOfWork.MobileUserGroupMatches.FindAsync(x => x.UserId == userId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<MobileUserGroupMatchDto>>(entities);
                return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievalError");
                return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>> GetByGroupCodeAsync(string groupCode)
        {
            try
            {
                var entities = await _unitOfWork.MobileUserGroupMatches.FindAsync(x => x.GroupCode == groupCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<MobileUserGroupMatchDto>>(entities);
                return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobileUserGroupMatchRetrievalError");
                return ApiResponse<IEnumerable<MobileUserGroupMatchDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobileUserGroupMatchDto>> CreateAsync(CreateMobileUserGroupMatchDto createDto)
        {
            try
            {
                var entity = _mapper.Map<MobileUserGroupMatch>(createDto);
                entity.CreatedDate = DateTimeProvider.Now;

                await _unitOfWork.MobileUserGroupMatches.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobileUserGroupMatchDto>(entity);
                return ApiResponse<MobileUserGroupMatchDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobileUserGroupMatchCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobileUserGroupMatchCreationError");
                return ApiResponse<MobileUserGroupMatchDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobileUserGroupMatchDto>> UpdateAsync(long id, UpdateMobileUserGroupMatchDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.MobileUserGroupMatches.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobileUserGroupMatchNotFound");
                    return ApiResponse<MobileUserGroupMatchDto>.ErrorResult(message, message, 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;

                _unitOfWork.MobileUserGroupMatches.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobileUserGroupMatchDto>(entity);
                return ApiResponse<MobileUserGroupMatchDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobileUserGroupMatchUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobileUserGroupMatchUpdateError");
                return ApiResponse<MobileUserGroupMatchDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.MobileUserGroupMatches.ExistsAsync(id);
                if (!exists)
                {
                    var message = _localizationService.GetLocalizedString("MobileUserGroupMatchNotFound");
                    return ApiResponse<bool>.ErrorResult(message, message, 404);
                }

                await _unitOfWork.MobileUserGroupMatches.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("MobileUserGroupMatchDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobileUserGroupMatchDeletionError");
                return ApiResponse<bool>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }
    }
}
