namespace Admin.Data
{
    public interface IIdentityStoreDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
