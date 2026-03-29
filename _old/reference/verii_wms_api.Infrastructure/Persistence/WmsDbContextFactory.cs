using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WMS_WEBAPI.Data
{
    public class WmsDbContextFactory : IDesignTimeDbContextFactory<WmsDbContext>
    {
        public WmsDbContext CreateDbContext(string[] args)
        {
            var basePath = ResolveConfigurationBasePath();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Data Source=wms.db";

            var optionsBuilder = new DbContextOptionsBuilder<WmsDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WmsDbContext(optionsBuilder.Options);
        }

        private static string ResolveConfigurationBasePath()
        {
            var current = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (current is not null)
            {
                var apiProjectPath = Path.Combine(current.FullName, "verii_wms_api.Api", "appsettings.json");
                if (File.Exists(apiProjectPath))
                {
                    return Path.GetDirectoryName(apiProjectPath)!;
                }

                var localSettingsPath = Path.Combine(current.FullName, "appsettings.json");
                if (File.Exists(localSettingsPath))
                {
                    return current.FullName;
                }

                current = current.Parent;
            }

            throw new InvalidOperationException("appsettings.json bulunamadi. Design-time DbContext olusturulamiyor.");
        }
    }
}
