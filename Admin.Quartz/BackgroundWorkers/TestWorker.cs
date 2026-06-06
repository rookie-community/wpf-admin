using Microsoft.Extensions.Logging;
using Quartz;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.DependencyInjection;

namespace Admin.BackgroundWorkers
{
    [DisallowConcurrentExecution]
    public class TestWorker : QuartzBackgroundWorkerBase, ITransientDependency
    {
        private readonly ILogger<TestWorker> _logger;

        public TestWorker(ILogger<TestWorker> logger)
        {
            //AutoRegister = false;//关闭自动注册，由数据库配置

            //手动配置
            JobDetail = JobBuilder.Create<TestWorker>().WithIdentity("TestWorker").Build();

            // 每 10 秒执行一次（你就能看到执行日志了）
            Trigger = TriggerBuilder.Create()
                .WithIdentity("TestWorkerTrigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever()
                    .WithMisfireHandlingInstructionNowWithExistingCount()//超时立即执行
                    ).Build();
            _logger = logger;
        }

        public override Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"[{nameof(TestWorker)}]在执行！10秒一次！");
            return Task.CompletedTask;
        }
    }
}
