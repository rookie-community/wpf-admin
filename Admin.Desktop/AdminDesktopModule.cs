using Admin.HttpApi.Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Configuration;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Admin.Desktop
{
    [DependsOn(
        typeof(AdminHttpApiClientModule),
        typeof(AdminApplicationContractsModule),
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientModule)
    )]
    public class AdminDesktopModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpHttpClientBuilderOptions>(options =>
            {
                options.ProxyClientBuildActions.Add((remoteServiceName, clientBuilder) =>
                {
                    clientBuilder.AddTransientHttpErrorPolicy(
                        policyBuilder => policyBuilder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
                    );
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var baseUrl = ConfigurationManager.AppSettings["RemoteServices"] ?? string.Empty;
            context.Services.Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default =
                    new RemoteServiceConfiguration(baseUrl);
            });

            context.Services.AddHttpClientProxies(
                typeof(AdminApplicationContractsModule).Assembly
            );
        }
    }
}
