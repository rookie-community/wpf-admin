using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Volo.Abp.EntityFrameworkCore;

namespace Admin.EntityFrameworkCore.EntityFrameworkCore
{
    public class AdminEfCoreDbContextFactory : IDesignTimeDbContextFactory<AdminEfCoreDbContext>
    {
        public AdminEfCoreDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<AdminEfCoreDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new AdminEfCoreDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Admin.HttpApi.Host/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
