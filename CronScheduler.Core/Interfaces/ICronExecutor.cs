namespace CronScheduler.Core.Interfaces;

public interface ICronExecutor
{
    /// <summary>
    /// The method that will be executed by the cron job when you implement the <c>ICronExecutor</c> and pass an instance to the <c>SchedulerOptionsBuilder</c>.
    /// </summary>
    void Execute();
}