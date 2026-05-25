using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace Admin
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AdminDomainSharedModule)
        )]
    public class AdminDomainModule : AbpModule
    {

    }
}
