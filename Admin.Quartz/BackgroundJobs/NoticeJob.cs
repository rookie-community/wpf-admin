using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Admin.BackgroundJobs
{
    public class NoticeJob : BackgroundJob<string>, ITransientDependency
    {
        public override void Execute(string args)
        {
            throw new NotImplementedException();

            //业务层入队（延后30秒执行）
            //await _backgroundJobManager.EnqueueAsync<NoticeJob, string>("消息", TimeSpan.FromSeconds(30));
        }
    }
}
