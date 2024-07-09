using CronScheduler.Core.Interfaces;

namespace CronScheduler.Core.Options;

internal class SchedulerOptions(string cronExpression) : ISchedulerOptions
{
    public string CronExpression { get; } = cronExpression;

    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now.AddYears(1);
    public ICronExecutor? CronExecutor { get; set; }
    public Action? ActionToExecute { get; set; }
    public int? NumberOfTimesToExecute { get; set; }
}