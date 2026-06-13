using Admin.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Admin.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AdminEntityFrameworkCoreModule),
    //typeof(AdminMongoDbModule),
    typeof(AdminApplicationContractsModule)
)]
public class AdminDbMigratorModule : AbpModule
{
}
