using CronScheduler.Core.CronJobs;

namespace CronScheduler.Core.Interfaces;

public interface ICronScheduler
{
    ICronJob ScheduleJob(ISchedulerOptions options);
    IEnumerable<Guid> GetRunningJobs();
    void StopJob(Guid jobId);
}