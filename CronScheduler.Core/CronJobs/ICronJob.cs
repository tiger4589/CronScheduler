namespace CronScheduler.Core.CronJobs;

public interface ICronJob
{
    /// <summary>
    /// The job's unique identifier.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Returns true if the job is running.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Fires when the job is executed.
    /// </summary>
    event EventHandler<Guid>? OnJobExecuted;

    /// <summary>
    /// Fires when there's no more occurrences in the defined period of the job.
    /// </summary>
    event EventHandler<Guid>? OnJobNoMoreOccurrences;

    /// <summary>
    /// Fires when the job has reached the maximum number of executions provided.
    /// </summary>
    event EventHandler<Guid>? OnJobMaxExecution;

    /// <summary>
    /// Fires when an exception is thrown while executing the job.
    /// </summary>
    event EventHandler<Exception>? OnErrorExecuting;
}