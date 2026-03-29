using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Models.UserPermissions;

namespace WMS_WEBAPI.Security
{
    public static class PermissionCatalogBootstrapper
    {
        public static async Task EnsureSeededAsync(WmsDbContext dbContext, CancellationToken cancellationToken = default)
        {
            var allCodes = PermissionCatalog.All.Select(x => x.Code).ToList();
            var existing = await dbContext.PermissionDefinitions
                .IgnoreQueryFilters()
                .Where(x => allCodes.Contains(x.Code))
                .ToListAsync(cancellationToken);

            var existingByCode = existing.ToDictionary(x => x.Code, StringComparer.OrdinalIgnoreCase);

            foreach (var definition in PermissionCatalog.All)
            {
                if (existingByCode.TryGetValue(definition.Code, out var entity))
                {
                    var changed = false;

                    if (entity.IsDeleted)
                    {
                        entity.IsDeleted = false;
                        entity.DeletedDate = null;
                        entity.DeletedBy = null;
                        changed = true;
                    }

                    if (!entity.IsActive)
                    {
                        entity.IsActive = true;
                        changed = true;
                    }

                    if (!string.Equals(entity.Name, definition.Name, StringComparison.Ordinal))
                    {
                        entity.Name = definition.Name;
                        changed = true;
                    }

                    if (!string.Equals(entity.Description, definition.Description, StringComparison.Ordinal))
                    {
                        entity.Description = definition.Description;
                        changed = true;
                    }

                    if (changed)
                    {
                        dbContext.PermissionDefinitions.Update(entity);
                    }

                    continue;
                }

                dbContext.PermissionDefinitions.Add(new PermissionDefinition
                {
                    Code = definition.Code,
                    Name = definition.Name,
                    Description = definition.Description,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTimeProvider.Now
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
