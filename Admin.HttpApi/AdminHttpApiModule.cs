using Volo.Abp.Http;
using Volo.Abp.Modularity;

namespace Admin.HttpApi
{
    [DependsOn(
        typeof(AbpHttpModule),
        typeof(AdminApplicationContractsModule)
        )]
    public class AdminHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
