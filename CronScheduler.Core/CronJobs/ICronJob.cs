namespace CronScheduler.Core.CronJobs;

public interface ICronJob
{
    Guid Id { get; }
    bool IsRunning { get; }

    event EventHandler<Guid>? OnJobExecuted;
    event EventHandler<Guid>? OnJobNoMoreOccurrences;
    event EventHandler<Guid>? OnJobMaxExecution;
    event EventHandler<Exception>? OnErrorExecuting;
}