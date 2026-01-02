using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace Admin
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AdminDomainSharedModule)
        )]
    public class AdminApplicationContractsModule: AbpModule
    {

    }
}
