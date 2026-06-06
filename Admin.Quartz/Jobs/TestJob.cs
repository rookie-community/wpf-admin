using Microsoft.Extensions.Logging;
using Quartz;
using Volo.Abp.DependencyInjection;

namespace Admin.Jobs
{
    public class TestJob : IJob, ITransientDependency
    {
        private readonly ILogger<TestJob> _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
