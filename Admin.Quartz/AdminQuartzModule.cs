using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace Admin
{
    [DependsOn(
        typeof(AbpQuartzModule),
        typeof(AbpBackgroundJobsQuartzModule),
        typeof(AbpBackgroundWorkersQuartzModule),
        typeof(AdminDomainModule),
        typeof(AdminApplicationContractsModule)
        )]
    public class AdminQuartzModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            PreConfigure<AbpQuartzOptions>(options =>
            {
                options.Configurator = configure =>
                {
                    configure.UseInMemoryStore();

                    // 持久化配置（以 SQL Server 为例）
                    //configure.UsePersistentStore(storeOptions =>
                    //{
                    //    storeOptions.UseProperties = true;
                    //    storeOptions.UseSqlServer(configuration.GetConnectionString("Quartz"));
                    //    storeOptions.UseClustering(c =>
                    //    {
                    //        c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                    //        c.CheckinInterval = TimeSpan.FromSeconds(10);
                    //    });
                    //});

                    // 线程池配置（强类型）
                    configure.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency = 10;
                    });
                };
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobQuartzOptions>(options =>
            {
                options.RetryCount = 1;
                options.RetryIntervalMillisecond = 1000;
            });

            // BackgroundJobs 启用
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.IsJobExecutionEnabled = true;
            });

            // BackgroundWorkers 配置
            Configure<AbpBackgroundWorkerQuartzOptions>(options =>
            {
                options.IsAutoRegisterEnabled = true; // 自动注册 Worker
            });
        }
    }
}
