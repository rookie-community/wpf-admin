namespace Admin.Data
{
    public interface IAdminStoreDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
