using Volo.Abp.Application;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace Admin
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuditingContractsModule),
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AdminDomainSharedModule)
        )]
    public class AdminApplicationContractsModule : AbpModule
    {

    }
}
