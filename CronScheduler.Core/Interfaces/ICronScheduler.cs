using CronScheduler.Core.CronJobs;

namespace CronScheduler.Core.Interfaces;

public interface ICronScheduler
{
    /// <summary>
    /// Schedule a cron job with the given options. 
    /// </summary>
    /// <param name="options">Use the <c>SchedulerOptionsBuilder</c> to create the options instance. For example:
    /// <code>
    ///    new SchedulerOptionsBuilder("CronExpressionHere")
    ///    .WithEndDate() //one of the building methods, you can chain them and use multiple ones
    ///    .Build(); //always end with .Build();
    /// </code>
    /// </param>
    /// <returns>An instance of ICronJob that has a unique ID and multiple events to listen to</returns>
    ICronJob ScheduleJob(ISchedulerOptions options);

    /// <summary>
    /// Get all running jobs ids at the moment.
    /// </summary>
    /// <returns>A list of GUIDs representing the unique ids of each running job</returns>
    IEnumerable<Guid> GetRunningJobs();

    /// <summary>
    /// Stop a running job using the unique id.
    /// </summary>
    /// <param name="jobId">The job unique id</param>
    void StopJob(Guid jobId);
}