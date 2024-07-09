using CronScheduler.Core.Interfaces;
using CronScheduler.Core.Options;

namespace CronScheduler.Core.CronJobs;

internal class ExecutorCronJob(SchedulerOptions options) : CronJob(options)
{
    private readonly ICronExecutor _executor = options.CronExecutor ?? throw new ArgumentNullException(nameof(options.CronExecutor));

    protected override void ExecuteJob()
    {
        _executor.Execute();
    }
}