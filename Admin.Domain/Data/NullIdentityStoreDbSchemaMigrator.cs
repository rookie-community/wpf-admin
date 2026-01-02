using Volo.Abp.DependencyInjection;

namespace Admin.Data
{
    public class NullIdentityStoreDbSchemaMigrator : IIdentityStoreDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
