using CronScheduler.Core.Interfaces;

namespace CronScheduler.Core.Options;

public class SchedulerOptionsBuilder(string cronExpression)
{
    private readonly SchedulerOptions _options = new(cronExpression);

    /// <summary>
    /// Build the options
    /// </summary>
    /// <returns>Returns the options instance that you can send to the <c>ICronScheduler</c></returns>
    public ISchedulerOptions Build() => _options;

    /// <summary>
    /// Sets the start date for the scheduler
    /// </summary>
    /// <param name="startDate">The date you want your cron job to start executing even if it has previous occurrences.</param>
    /// <returns>Returns the instance of <c>SchedulerOptionsBuilder</c> you're using to build the options.</returns>
    public SchedulerOptionsBuilder WithStartDate(DateTime startDate)
    {
        _options.StartDate = startDate;
        return this;
    }

    /// <summary>
    /// Sets the end date for the scheduler
    /// </summary>
    /// <param name="endDate">The date you want your cron job to stop executing even if it has later occurrences.</param>
    /// <returns>Returns the instance of <c>SchedulerOptionsBuilder</c> you're using to build the options.</returns>
    public SchedulerOptionsBuilder WithEndDate(DateTime endDate)
    {
        _options.EndDate = endDate;
        return this;
    }

    /// <summary>
    /// Sets both the start and end date for the scheduler
    /// </summary>
    /// <param name="startDate">The date you want your cron job to start executing even if it has previous occurrences.</param>
    /// <param name="endDate">The date you want your cron job to stop executing even if it has later occurrences.</param>
    /// <returns>Returns the instance of <c>SchedulerOptionsBuilder</c> you're using to build the options.</returns>
    public SchedulerOptionsBuilder WithStartAndEndDate(DateTime startDate, DateTime endDate)
    {
        _options.StartDate = startDate;
        _options.EndDate = endDate;
        return this;
    }

    /// <summary>
    /// Sets the instance of <c>ICronExecutor</c> that will be used to execute the implemented <c>Execute</c> method in your class.
    /// <remarks>Either a <c>ICronExecutor</c> or an <c>Action</c> can be set. If you set both, an exception will be thrown. One of them is required though.</remarks>
    /// </summary>
    /// <param name="executor">The instance of your class that implements <c>ICronExecutor</c></param>
    /// <returns>Returns the instance of <c>SchedulerOptionsBuilder</c> you're using to build the options.</returns>
    public SchedulerOptionsBuilder WithCronExecutor(ICronExecutor executor)
    {
        _options.CronExecutor = executor;
        return this;
    }

    /// <summary>
    /// Sets the <c>Action</c> that will execute.
    /// <remarks>Either a <c>ICronExecutor</c> or an <c>Action</c> can be set. If you set both, an exception will be thrown. One of them is required though.</remarks>
    /// </summary>
    /// <param name="action">The action to execute</param>
    /// <returns>Returns the instance of <c>SchedulerOptionsBuilder</c> you're using to build the options.</returns>
    public SchedulerOptionsBuilder WithActionToExecute(Action action)
    {
        _options.ActionToExecute = action;
        return this;
    }

    /// <summary>
    /// Sets the number of times the scheduler should execute regardless of how many occurrences was found.
    /// </summary>
    /// <param name="numberOfTimes">Max number of executions</param>
    /// <returns>Returns the instance of <c>SchedulerOptionsBuilder</c> you're using to build the options.</returns>
    public SchedulerOptionsBuilder WithNumberOfTimesToExecute(int numberOfTimes)
    {
        _options.NumberOfTimesToExecute = numberOfTimes;
        return this;
    }
}