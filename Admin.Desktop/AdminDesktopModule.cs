using Admin.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Admin.Desktop
{
    [DependsOn(
        typeof(AdminApplicationModule),
        typeof(AdminEntityFrameworkCoreModule)
        )]
    public class AdminDesktopModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
