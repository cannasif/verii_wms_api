using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(WmsDbContext context)
        {
            // Veritabanının oluşturulduğundan emin ol
            context.Database.EnsureCreated();

            try
            {
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (
                        SELECT 1
                        FROM sys.default_constraints dc
                        INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
                        WHERE dc.parent_object_id = OBJECT_ID('[dbo].[RII_USER_SESSION]') AND c.name = 'IsDeleted'
                    )
                    BEGIN
                        ALTER TABLE [dbo].[RII_USER_SESSION]
                        ADD CONSTRAINT [DF_RII_USER_SESSION_IsDeleted] DEFAULT(0) FOR [IsDeleted];
                    END

                    UPDATE [dbo].[RII_USER_SESSION]
                    SET [IsDeleted] = 0
                    WHERE [IsDeleted] IS NULL;

                    IF OBJECT_ID('[dbo].[RII_PASSWORD_RESET_REQUEST]', 'U') IS NULL
                    BEGIN
                        CREATE TABLE [dbo].[RII_PASSWORD_RESET_REQUEST] (
                            [Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            [CreatedDate] DATETIME2 NULL,
                            [UpdatedDate] DATETIME2 NULL,
                            [DeletedDate] DATETIME2 NULL,
                            [IsDeleted] BIT NOT NULL CONSTRAINT [DF_RII_PASSWORD_RESET_REQUEST_IsDeleted] DEFAULT(0),
                            [CreatedBy] BIGINT NULL,
                            [UpdatedBy] BIGINT NULL,
                            [DeletedBy] BIGINT NULL,
                            [UserId] BIGINT NOT NULL,
                            [TokenHash] NVARCHAR(128) NOT NULL,
                            [ExpiresAt] DATETIME2 NOT NULL,
                            [UsedAt] DATETIME2 NULL,
                            [RequestIp] NVARCHAR(100) NULL,
                            [UserAgent] NVARCHAR(500) NULL
                        );
                        ALTER TABLE [dbo].[RII_PASSWORD_RESET_REQUEST] WITH CHECK ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_UserId] FOREIGN KEY([UserId])
                        REFERENCES [dbo].[RII_USERS] ([Id]);
                        CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_UserId_TokenHash] ON [dbo].[RII_PASSWORD_RESET_REQUEST] ([UserId], [TokenHash]);
                        CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_ExpiresAt] ON [dbo].[RII_PASSWORD_RESET_REQUEST] ([ExpiresAt]);
                    END
                    ");
            }
            catch { }

            var now = DateTime.UtcNow;
            var allowedRoleTitles = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "user", "admin" };

            var authorities = context.UserAuthorities.IgnoreQueryFilters().ToList();
            var userAuthority = authorities.FirstOrDefault(a => string.Equals(a.Title, "user", StringComparison.OrdinalIgnoreCase));
            var adminAuthority = authorities.FirstOrDefault(a => string.Equals(a.Title, "admin", StringComparison.OrdinalIgnoreCase));

            if (userAuthority == null)
            {
                userAuthority = new UserAuthority { Id = 1, Title = "user", CreatedDate = now, IsDeleted = false };
                context.UserAuthorities.Add(userAuthority);
            }
            else if (userAuthority.IsDeleted)
            {
                userAuthority.IsDeleted = false;
                userAuthority.DeletedDate = null;
                context.UserAuthorities.Update(userAuthority);
            }

            if (adminAuthority == null)
            {
                adminAuthority = new UserAuthority { Id = 2, Title = "admin", CreatedDate = now, IsDeleted = false };
                context.UserAuthorities.Add(adminAuthority);
            }
            else if (adminAuthority.IsDeleted)
            {
                adminAuthority.IsDeleted = false;
                adminAuthority.DeletedDate = null;
                context.UserAuthorities.Update(adminAuthority);
            }

            context.SaveChanges();

            var fallbackAdminRoleId = adminAuthority?.Id ?? 2;
            var unsupportedAuthorities = context.UserAuthorities
                .IgnoreQueryFilters()
                .Where(a => !allowedRoleTitles.Contains(a.Title))
                .ToList();

            if (unsupportedAuthorities.Count > 0)
            {
                var unsupportedIds = unsupportedAuthorities.Select(a => a.Id).ToHashSet();
                var affectedUsers = context.Users.Where(u => unsupportedIds.Contains(u.RoleId)).ToList();
                foreach (var user in affectedUsers)
                {
                    user.RoleId = fallbackAdminRoleId;
                }

                foreach (var authority in unsupportedAuthorities)
                {
                    authority.IsDeleted = true;
                    authority.DeletedDate = now;
                }

                context.SaveChanges();
            }

            // Admin kullanıcısı zaten var mı kontrol et
            if (context.Users.Any(u => u.Email == "admin@v3rii.com"))
            {
                return; // Zaten var, seed data'ya gerek yok
            }

            // Admin kullanıcısını oluştur
            var adminUser = new User
            {
                Username = "admin@v3rii.com",
                Email = "admin@v3rii.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Veriipass123!"),
                FirstName = "Admin",
                LastName = "User",
                RoleId = fallbackAdminRoleId,
                IsEmailConfirmed = true,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}
