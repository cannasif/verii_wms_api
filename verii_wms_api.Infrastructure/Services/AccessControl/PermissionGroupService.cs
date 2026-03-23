using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models.UserPermissions;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PermissionGroupService : IPermissionGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;

        public PermissionGroupService(IUnitOfWork unitOfWork, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<PermissionGroupDto>>> GetAllAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? nameof(PermissionGroup.Id) : request.SortBy;
                var desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                var query = _unitOfWork.PermissionGroups.Query()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .Include(x => x.GroupPermissions.Where(gp => !gp.IsDeleted))
                    .ThenInclude(x => x.PermissionDefinition)
                    .ApplyFilters(request.Filters)
                    .ApplySorting(sortBy, desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtoItems = items.Select(MapToDto).ToList();
                var paged = new PagedResponse<PermissionGroupDto>(dtoItems, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PermissionGroupDto>>.SuccessResult(
                    paged,
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PermissionGroupDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionGroupDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PermissionGroups.Query()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .Include(x => x.GroupPermissions.Where(gp => !gp.IsDeleted))
                    .ThenInclude(x => x.PermissionDefinition)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return ApiResponse<PermissionGroupDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<PermissionGroupDto>.SuccessResult(
                    MapToDto(entity),
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionGroupDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetById"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionGroupDto>> CreateAsync(CreatePermissionGroupDto dto)
        {
            try
            {
                var duplicate = await _unitOfWork.PermissionGroups.Query()
                    .Where(x => !x.IsDeleted && x.Name == dto.Name)
                            .AnyAsync();

                if (duplicate)
                {
                    return ApiResponse<PermissionGroupDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status400BadRequest);
                }

                var entity = new PermissionGroup
                {
                    Name = dto.Name.Trim(),
                    Description = dto.Description?.Trim(),
                    IsSystemAdmin = dto.IsSystemAdmin,
                    IsActive = dto.IsActive
                };

                await _unitOfWork.PermissionGroups.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                if (dto.PermissionDefinitionIds.Count > 0)
                {
                    var linkResult = await SetPermissionsInternalAsync(entity.Id, dto.PermissionDefinitionIds);
                    if (!linkResult.Success)
                    {
                        return ApiResponse<PermissionGroupDto>.ErrorResult(linkResult.Message, linkResult.ExceptionMessage, linkResult.StatusCode);
                    }
                }

                return await GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionGroupDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_Create"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionGroupDto>> UpdateAsync(long id, UpdatePermissionGroupDto dto)
        {
            try
            {
                var entity = await _unitOfWork.PermissionGroups.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return ApiResponse<PermissionGroupDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status404NotFound);
                }

                if (entity.IsSystemAdmin)
                {
                    return ApiResponse<PermissionGroupDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        "System Admin permission group cannot be modified.",
                        StatusCodes.Status403Forbidden);
                }

                if (!string.IsNullOrWhiteSpace(dto.Name) && !dto.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var duplicate = await _unitOfWork.PermissionGroups.Query()
                        .Where(x => !x.IsDeleted && x.Id != id && x.Name == dto.Name)
                            .AnyAsync();

                    if (duplicate)
                    {
                        return ApiResponse<PermissionGroupDto>.ErrorResult(
                            _localizationService.GetLocalizedString("ValidationError"),
                            _localizationService.GetLocalizedString("ValidationError"),
                            StatusCodes.Status400BadRequest);
                    }

                    entity.Name = dto.Name.Trim();
                }

                if (dto.Description != null)
                {
                    entity.Description = dto.Description.Trim();
                }

                if (dto.IsSystemAdmin.HasValue)
                {
                    entity.IsSystemAdmin = dto.IsSystemAdmin.Value;
                }

                if (dto.IsActive.HasValue)
                {
                    entity.IsActive = dto.IsActive.Value;
                }

                _unitOfWork.PermissionGroups.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return await GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionGroupDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_Update"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionGroupDto>> SetPermissionsAsync(long id, SetPermissionGroupPermissionsDto dto)
        {
            var group = await _unitOfWork.PermissionGroups.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
            if (group != null && group.IsSystemAdmin)
            {
                return ApiResponse<PermissionGroupDto>.ErrorResult(
                    _localizationService.GetLocalizedString("ValidationError"),
                    "System Admin permission group cannot be modified.",
                    StatusCodes.Status403Forbidden);
            }

            var result = await SetPermissionsInternalAsync(id, dto.PermissionDefinitionIds);
            if (!result.Success)
            {
                return ApiResponse<PermissionGroupDto>.ErrorResult(result.Message, result.ExceptionMessage, result.StatusCode);
            }

            return await GetByIdAsync(id);
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PermissionGroups.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status404NotFound);
                }

                if (entity.IsSystemAdmin)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        "System Admin permission group cannot be deleted.",
                        StatusCodes.Status403Forbidden);
                }

                await _unitOfWork.PermissionGroups.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_Delete"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<ApiResponse<bool>> SetPermissionsInternalAsync(long groupId, List<long> permissionIds)
        {
            try
            {
                var group = await _unitOfWork.PermissionGroups.GetByIdAsync(groupId);
                if (group == null)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status404NotFound);
                }

                var distinctPermissionIds = permissionIds.Distinct().ToList();
                if (distinctPermissionIds.Count > 0)
                {
                    var validCount = await _unitOfWork.PermissionDefinitions.Query()
                        .Where(x => !x.IsDeleted && distinctPermissionIds.Contains(x.Id))
                            .CountAsync();

                    if (validCount != distinctPermissionIds.Count)
                    {
                        return ApiResponse<bool>.ErrorResult(
                            _localizationService.GetLocalizedString("ValidationError"),
                            _localizationService.GetLocalizedString("ValidationError"),
                            StatusCodes.Status400BadRequest);
                    }
                }

                var currentLinks = await _unitOfWork.PermissionGroupPermissions.Query(ignoreQueryFilters: true)
                    
                    .Where(x => x.PermissionGroupId == groupId)
                    .ToListAsync();

                foreach (var link in currentLinks.Where(x => !x.IsDeleted && !distinctPermissionIds.Contains(x.PermissionDefinitionId)))
                {
                    await _unitOfWork.PermissionGroupPermissions.SoftDelete(link.Id);
                }

                foreach (var permissionId in distinctPermissionIds)
                {
                    var existing = currentLinks.FirstOrDefault(x => x.PermissionDefinitionId == permissionId);
                    if (existing == null)
                    {
                        await _unitOfWork.PermissionGroupPermissions.AddAsync(new PermissionGroupPermission
                        {
                            PermissionGroupId = groupId,
                            PermissionDefinitionId = permissionId
                        });
                        continue;
                    }

                    if (existing.IsDeleted)
                    {
                        existing.IsDeleted = false;
                        existing.DeletedDate = null;
                        existing.DeletedBy = null;
                        _unitOfWork.PermissionGroupPermissions.Update(existing);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_Update"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        private static PermissionGroupDto MapToDto(PermissionGroup entity)
        {
            var groupPermissions = entity.GroupPermissions
                .Where(x => !x.IsDeleted && x.PermissionDefinition != null && !x.PermissionDefinition.IsDeleted && x.PermissionDefinition.IsActive)
                .ToList();

            return new PermissionGroupDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsSystemAdmin = entity.IsSystemAdmin,
                IsActive = entity.IsActive,
                PermissionDefinitionIds = groupPermissions.Select(x => x.PermissionDefinitionId).Distinct().OrderBy(x => x).ToList(),
                PermissionCodes = groupPermissions.Select(x => x.PermissionDefinition.Code).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList(),
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                DeletedDate = entity.DeletedDate,
                IsDeleted = entity.IsDeleted,
                CreatedBy = entity.CreatedBy,
                UpdatedBy = entity.UpdatedBy,
                DeletedBy = entity.DeletedBy,
                CreatedByFullUser = entity.CreatedByUser != null ? $"{entity.CreatedByUser.FirstName} {entity.CreatedByUser.LastName}".Trim() : null,
                UpdatedByFullUser = entity.UpdatedByUser != null ? $"{entity.UpdatedByUser.FirstName} {entity.UpdatedByUser.LastName}".Trim() : null,
                DeletedByFullUser = entity.DeletedByUser != null ? $"{entity.DeletedByUser.FirstName} {entity.DeletedByUser.LastName}".Trim() : null
            };
        }
    }
}
