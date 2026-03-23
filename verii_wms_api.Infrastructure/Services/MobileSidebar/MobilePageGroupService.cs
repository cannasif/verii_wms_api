using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class MobilePageGroupService : IMobilePageGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public MobilePageGroupService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.MobilePageGroups.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<MobilePageGroupDto>>(entities);
                return ApiResponse<IEnumerable<MobilePageGroupDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilePageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred");
                return ApiResponse<IEnumerable<MobilePageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<MobilePageGroupDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.MobilePageGroups.Query().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<MobilePageGroupDto>>(entities);
                var result = new PagedResponse<MobilePageGroupDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<MobilePageGroupDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("MobilePageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<MobilePageGroupDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<MobilePageGroupDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.MobilePageGroups.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilePageGroupNotFound");
                    return ApiResponse<MobilePageGroupDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilePageGroupNotFound"), 404);
                }

                var dto = _mapper.Map<MobilePageGroupDto>(entity);
                return ApiResponse<MobilePageGroupDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilePageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred");
                return ApiResponse<MobilePageGroupDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetByGroupCodeAsync(string groupCode)
        {
            try
            {
                var entities = await _unitOfWork.MobilePageGroups.FindAsync(x => x.GroupCode == groupCode && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<MobilePageGroupDto>>(entities);
                return ApiResponse<IEnumerable<MobilePageGroupDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilePageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred");
                return ApiResponse<IEnumerable<MobilePageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilePageGroupDto>> CreateAsync(CreateMobilePageGroupDto createDto)
        {
            try
            {
                var entity = _mapper.Map<MobilePageGroup>(createDto);
                entity.CreatedDate = DateTimeProvider.Now;

                await _unitOfWork.MobilePageGroups.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobilePageGroupDto>(entity);
                return ApiResponse<MobilePageGroupDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilePageGroupCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred");
                return ApiResponse<MobilePageGroupDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilePageGroupDto>> UpdateAsync(long id, UpdateMobilePageGroupDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.MobilePageGroups.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilePageGroupNotFound");
                    return ApiResponse<MobilePageGroupDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilePageGroupNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;

                _unitOfWork.MobilePageGroups.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobilePageGroupDto>(entity);
                return ApiResponse<MobilePageGroupDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilePageGroupUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred");
                return ApiResponse<MobilePageGroupDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.MobilePageGroups.ExistsAsync(id);
                if (!exists)
                {
                    var message = _localizationService.GetLocalizedString("MobilePageGroupNotFound");
                    return ApiResponse<bool>.ErrorResult(message, _localizationService.GetLocalizedString("MobilePageGroupNotFound"), 404);
                }

                await _unitOfWork.MobilePageGroups.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("MobilePageGroupDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred");
                return ApiResponse<bool>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobilePageGroupDto>>> GetMobilPageGroupsByGroupCodeAsync()
        {
            try
            {
                var groupedByGroupCode = await _unitOfWork.MobilePageGroups.Query()
                    .Where(pg => !pg.IsDeleted)
                    .GroupBy(pg => pg.GroupCode)
                    .Select(g => g.First())
                    .ToListAsync();

                var dtos = _mapper.Map<IEnumerable<MobilePageGroupDto>>(groupedByGroupCode);
                return ApiResponse<IEnumerable<MobilePageGroupDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilePageGroupRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilePageGroupErrorOccurred");
                return ApiResponse<IEnumerable<MobilePageGroupDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }
    }
}
