using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace Admin
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AdminApplicationContractsModule),
        typeof(AdminDomainModule)
        )]
    public class AdminApplicationModule: AbpModule
    {

    }
}
