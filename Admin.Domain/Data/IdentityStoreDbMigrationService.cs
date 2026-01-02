using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace Admin.Data
{
    public class IdentityStoreDbMigrationService : ITransientDependency
    {
        public ILogger<IdentityStoreDbMigrationService> Logger { get; set; }

        private readonly IDataSeeder _dataSeeder;
        private readonly IEnumerable<IIdentityStoreDbSchemaMigrator> _dbSchemaMigrators;
        private readonly ITenantRepository _tenantRepository;
        private readonly ICurrentTenant _currentTenant;

        public IdentityStoreDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IIdentityStoreDbSchemaMigrator> dbSchemaMigrators,
            ITenantRepository tenantRepository,
            ICurrentTenant currentTenant)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;
            _tenantRepository = tenantRepository;
            _currentTenant = currentTenant;

            Logger = NullLogger<IdentityStoreDbMigrationService>.Instance;
        }

        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
