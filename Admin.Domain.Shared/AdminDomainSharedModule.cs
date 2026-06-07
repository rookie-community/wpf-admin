using Volo.Abp.AuditLogging;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace Admin
{
    [DependsOn(
        typeof(AbpDddDomainSharedModule),
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpTenantManagementDomainSharedModule)
        )]
    public class AdminDomainSharedModule : AbpModule
    {

    }
}
