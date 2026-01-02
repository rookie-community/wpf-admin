using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace Admin
{
    [DependsOn(
        typeof(AbpDddDomainSharedModule),
        typeof(AbpTenantManagementDomainSharedModule)
        )]
    public class AdminDomainSharedModule : AbpModule
    {

    }
}
