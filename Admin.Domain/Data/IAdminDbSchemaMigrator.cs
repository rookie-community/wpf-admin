using System.Threading.Tasks;

namespace Admin.Data;

public interface IAdminDbSchemaMigrator
{
    Task MigrateAsync();
}
