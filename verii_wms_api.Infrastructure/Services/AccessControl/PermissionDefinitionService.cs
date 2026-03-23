using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models.UserPermissions;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PermissionDefinitionService : IPermissionDefinitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;

        public PermissionDefinitionService(IUnitOfWork unitOfWork, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<PagedResponse<PermissionDefinitionDto>>> GetAllAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var sortBy = string.IsNullOrWhiteSpace(request.SortBy) ? nameof(PermissionDefinition.Id) : request.SortBy;
                var desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

                var query = _unitOfWork.PermissionDefinitions.AsQueryable()
                    .AsNoTracking()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .ApplyFilters(request.Filters)
                    .ApplySorting(sortBy, desc);

                var totalCount = await query.CountAsync();
                var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync();

                var dtoItems = items.Select(MapToDto).ToList();
                var paged = new PagedResponse<PermissionDefinitionDto>(dtoItems, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PermissionDefinitionDto>>.SuccessResult(
                    paged,
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PermissionDefinitionDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionDefinitionDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PermissionDefinitions.Query()
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.UpdatedByUser)
                    .Include(x => x.DeletedByUser)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return ApiResponse<PermissionDefinitionDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status404NotFound);
                }

                return ApiResponse<PermissionDefinitionDto>.SuccessResult(
                    MapToDto(entity),
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionDefinitionDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetById"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionDefinitionDto>> CreateAsync(CreatePermissionDefinitionDto dto)
        {
            try
            {
                var exists = await _unitOfWork.PermissionDefinitions.AsQueryable()
                    .AsNoTracking()
                    .AnyAsync(x => !x.IsDeleted && x.Code == dto.Code);

                if (exists)
                {
                    return ApiResponse<PermissionDefinitionDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status400BadRequest);
                }

                var entity = new PermissionDefinition
                {
                    Code = dto.Code.Trim(),
                    Name = dto.Name.Trim(),
                    Description = dto.Description?.Trim(),
                    IsActive = dto.IsActive
                };

                await _unitOfWork.PermissionDefinitions.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                return await GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionDefinitionDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_Create"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionDefinitionDto>> UpdateAsync(long id, UpdatePermissionDefinitionDto dto)
        {
            try
            {
                var entity = await _unitOfWork.PermissionDefinitions.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return ApiResponse<PermissionDefinitionDto>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status404NotFound);
                }

                if (!string.IsNullOrWhiteSpace(dto.Code) && !dto.Code.Equals(entity.Code, StringComparison.OrdinalIgnoreCase))
                {
                    var duplicate = await _unitOfWork.PermissionDefinitions.AsQueryable()
                        .AsNoTracking()
                        .AnyAsync(x => !x.IsDeleted && x.Id != id && x.Code == dto.Code);

                    if (duplicate)
                    {
                        return ApiResponse<PermissionDefinitionDto>.ErrorResult(
                            _localizationService.GetLocalizedString("ValidationError"),
                            _localizationService.GetLocalizedString("ValidationError"),
                            StatusCodes.Status400BadRequest);
                    }

                    entity.Code = dto.Code.Trim();
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    entity.Name = dto.Name.Trim();
                }

                if (dto.Description != null)
                {
                    entity.Description = dto.Description.Trim();
                }

                if (dto.IsActive.HasValue)
                {
                    entity.IsActive = dto.IsActive.Value;
                }

                _unitOfWork.PermissionDefinitions.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                return await GetByIdAsync(entity.Id);
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionDefinitionDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_Update"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<PermissionDefinitionSyncResultDto>> SyncAsync(SyncPermissionDefinitionsDto dto)
        {
            try
            {
                dto ??= new SyncPermissionDefinitionsDto();
                dto.Items ??= new List<SyncPermissionDefinitionItemDto>();

                var normalized = dto.Items
                    .Where(x => x != null && !string.IsNullOrWhiteSpace(x.Code))
                    .Select(x => new SyncPermissionDefinitionItemDto
                    {
                        Code = x.Code.Trim(),
                        Name = string.IsNullOrWhiteSpace(x.Name) ? null : x.Name.Trim(),
                        Description = x.Description == null ? null : x.Description.Trim(),
                        IsActive = x.IsActive
                    })
                    .ToList();

                var distinctCodes = normalized
                    .Select(x => x.Code)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (distinctCodes.Count == 0)
                {
                    return ApiResponse<PermissionDefinitionSyncResultDto>.SuccessResult(
                        new PermissionDefinitionSyncResultDto { CreatedCount = 0, UpdatedCount = 0, ReactivatedCount = 0, TotalProcessed = 0 },
                        _localizationService.GetLocalizedString("OperationSuccessful"));
                }

                var existingAll = await _unitOfWork.PermissionDefinitions.AsQueryable()
                    .IgnoreQueryFilters()
                    .Where(x => distinctCodes.Contains(x.Code))
                    .ToListAsync();

                var existingByCode = existingAll
                    .GroupBy(x => x.Code, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

                var created = 0;
                var updated = 0;
                var reactivated = 0;

                foreach (var group in normalized.GroupBy(x => x.Code, StringComparer.OrdinalIgnoreCase))
                {
                    var item = group.First();

                    if (existingByCode.TryGetValue(item.Code, out var entity))
                    {
                        var reactivatedThisEntity = false;
                        if (dto.ReactivateSoftDeleted && entity.IsDeleted)
                        {
                            entity.IsDeleted = false;
                            entity.DeletedBy = null;
                            entity.DeletedDate = null;
                            reactivated++;
                            reactivatedThisEntity = true;
                        }

                        var changed = reactivatedThisEntity;

                        if (dto.UpdateExistingNames && !string.IsNullOrWhiteSpace(item.Name) && !string.Equals(entity.Name, item.Name, StringComparison.Ordinal))
                        {
                            entity.Name = item.Name;
                            changed = true;
                        }

                        if (dto.UpdateExistingDescriptions && item.Description != null && !string.Equals(entity.Description, item.Description, StringComparison.Ordinal))
                        {
                            entity.Description = item.Description;
                            changed = true;
                        }

                        if (dto.UpdateExistingIsActive && entity.IsActive != item.IsActive)
                        {
                            entity.IsActive = item.IsActive;
                            changed = true;
                        }

                        if (changed)
                        {
                            _unitOfWork.PermissionDefinitions.Update(entity);
                            updated++;
                        }

                        continue;
                    }

                    var name = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : item.Code;

                    var newEntity = new PermissionDefinition
                    {
                        Code = item.Code,
                        Name = name,
                        Description = item.Description,
                        IsActive = item.IsActive
                    };

                    await _unitOfWork.PermissionDefinitions.AddAsync(newEntity);
                    created++;
                }

                if (created > 0 || updated > 0 || reactivated > 0)
                {
                    await _unitOfWork.SaveChangesAsync();
                }

                return ApiResponse<PermissionDefinitionSyncResultDto>.SuccessResult(
                    new PermissionDefinitionSyncResultDto
                    {
                        CreatedCount = created,
                        UpdatedCount = updated,
                        ReactivatedCount = reactivated,
                        TotalProcessed = normalized.Count
                    },
                    _localizationService.GetLocalizedString("OperationSuccessful"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PermissionDefinitionSyncResultDto>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_Update"),
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PermissionDefinitions.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(
                        _localizationService.GetLocalizedString("ValidationError"),
                        _localizationService.GetLocalizedString("ValidationError"),
                        StatusCodes.Status404NotFound);
                }

                await _unitOfWork.PermissionDefinitions.SoftDelete(id);
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

        private static PermissionDefinitionDto MapToDto(PermissionDefinition entity)
        {
            return new PermissionDefinitionDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
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
