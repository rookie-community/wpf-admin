using Volo.Abp.DependencyInjection;

namespace Admin.Data
{
    public class NullAdminStoreDbSchemaMigrator : IAdminStoreDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
