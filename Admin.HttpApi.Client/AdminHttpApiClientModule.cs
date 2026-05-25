using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Admin.HttpApi.Client
{
    [DependsOn(
        typeof(AdminApplicationContractsModule),
        typeof(AbpHttpClientModule)
       )]
    public class AdminHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AdminApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
