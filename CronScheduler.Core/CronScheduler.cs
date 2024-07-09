using System.Collections.Concurrent;
using CronScheduler.Core.CronJobs;
using CronScheduler.Core.Interfaces;
using CronScheduler.Core.Options;

namespace CronScheduler.Core;

internal class CronScheduler : ICronScheduler
{
    private readonly ConcurrentBag<CronJob> _jobs = [];

    public ICronJob ScheduleJob(ISchedulerOptions options)
    {
        var schedulerOptions = GetSchedulerOptions(options);
        var job = CreateCronJob(schedulerOptions);
        var isRunning = job.ScheduleNextRun();

        if (!isRunning)
        {
            throw new InvalidOperationException($"No occurence found between {schedulerOptions.StartDate} and {schedulerOptions.EndDate} for the following expression: {schedulerOptions.CronExpression}");
        }

        _jobs.Add(job);
        return job;
    }

    public IEnumerable<Guid> GetRunningJobs()
    {
        return from cronJob in _jobs where cronJob.IsRunning select cronJob.Id;
    }

    public void StopJob(Guid jobId)
    {
        var job = _jobs.FirstOrDefault(job => job.Id == jobId) ?? throw new ArgumentException($"Job with id {jobId} not found");
        job.StopJob();
    }

    private static SchedulerOptions GetSchedulerOptions(ISchedulerOptions options)
    {
        if (options is not SchedulerOptions schedulerOptions)
        {
            throw new ArgumentNullException(nameof(options));
        }

        ValidateOptions(schedulerOptions);
        return schedulerOptions;
    }

    private static CronJob CreateCronJob(SchedulerOptions schedulerOptions)
    {
        if (schedulerOptions.CronExecutor is not null)
        {
            return new ExecutorCronJob(schedulerOptions);
        }

        return new ActionCronJob(schedulerOptions);
    }

    private static void ValidateOptions(SchedulerOptions schedulerOptions)
    {
        if (schedulerOptions.CronExecutor is null && schedulerOptions.ActionToExecute is null)
        {
            throw new ArgumentException("Either a CronExecutor or an Action must be provided");
        }

        if (schedulerOptions.CronExecutor is not null && schedulerOptions.ActionToExecute is not null)
        {
            throw new ArgumentException("Both a CronExecutor and an Action cannot be provided");
        }

        if (schedulerOptions.StartDate > schedulerOptions.EndDate)
        {
            throw new ArgumentException("Start date cannot be greater than end date");
        }

        if (schedulerOptions.StartDate == schedulerOptions.EndDate)
        {
            throw new ArgumentException("Start date cannot be equal to end date");
        }
    }
}