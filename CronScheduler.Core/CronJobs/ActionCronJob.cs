using CronScheduler.Core.Options;

namespace CronScheduler.Core.CronJobs;

internal class ActionCronJob(SchedulerOptions options) : CronJob(options)
{
    private readonly Action _action = options.ActionToExecute ?? throw new ArgumentNullException(nameof(options.ActionToExecute));

    protected override void ExecuteJob()
    {
        _action.Invoke();
    }
}