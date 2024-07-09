using CronScheduler.Core.Options;
using NCrontab;

namespace CronScheduler.Core.CronJobs;

internal abstract class CronJob : ICronJob
{
    public Guid Id { get; } = Guid.NewGuid();
    public bool IsRunning => _jobTimer != null;

    public event EventHandler<Guid>? OnJobExecuted;
    public event EventHandler<Guid>? OnJobNoMoreOccurrences;
    public event EventHandler<Guid>? OnJobMaxExecution;
    public event EventHandler<Exception>? OnErrorExecuting;

    private CrontabSchedule? _crontabSchedule;
    private readonly DateTime _initialStartDate;
    private readonly DateTime _endDate;
    private readonly int? _numberOfTimesToExecute;

    private Timer? _jobTimer;
    private int _numberOfTimesExecuted;
    private DateTime StartDate => _initialStartDate < DateTime.Now ? DateTime.Now : _initialStartDate;

    internal CronJob(SchedulerOptions options)
    {

        SetCrontabSchedule(options);

        _initialStartDate = options.StartDate;
        _endDate = options.EndDate;
        _numberOfTimesToExecute = options.NumberOfTimesToExecute;
    }

    private void SetCrontabSchedule(SchedulerOptions options)
    {
        var cronValues = options.CronExpression.Split(' ');
        var includeSeconds = cronValues.Length == 6;
        _crontabSchedule = CrontabSchedule.TryParse(options.CronExpression, new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = includeSeconds
        }) ?? throw new ArgumentException("Invalid cron expression.");
    }

    internal bool ScheduleNextRun()
    {
        var occurrencesBySchedule = _crontabSchedule?.GetNextOccurrences(StartDate, _endDate).ToList();

        if (occurrencesBySchedule == null)
        {
            return false;
        }

        if (occurrencesBySchedule.Count == 0)
        {
            return false;
        }

        var nextTimeToFire = occurrencesBySchedule.First();
        var timespanToFire = nextTimeToFire - DateTime.Now;

        if (_jobTimer == null)
        {
            _jobTimer = new Timer(FireJob, null, timespanToFire, Timeout.InfiniteTimeSpan);
        }
        else
        {
            _jobTimer.Change(timespanToFire, Timeout.InfiniteTimeSpan);
        }

        return true;
    }

    private void RaiseExecuted()
    {
        var handler = OnJobExecuted;
        handler?.Invoke(this, Id);
    }

    private void RaiseMaxExecutionsReached()
    {
        var handler = OnJobMaxExecution;
        handler?.Invoke(this, Id);
    }

    private void RaiseNoMoreOccurrences()
    {
        var handler = OnJobNoMoreOccurrences;
        handler?.Invoke(this, Id);
    }

    private void RaiseError(Exception e)
    {
        var handler = OnErrorExecuting;
        handler?.Invoke(this, e);
    }

    private void FireJob(object? state)
    {
        try
        {
            ExecuteJob();
            RaiseExecuted();

            _numberOfTimesExecuted++;
        }
        catch (Exception e)
        {
            RaiseError(e);
        }

        if (_numberOfTimesExecuted == _numberOfTimesToExecute)
        {
            RaiseMaxExecutionsReached();
            DisposeTimer();
            return;
        }

        var b = ScheduleNextRun();
        if (!b)
        {
            RaiseNoMoreOccurrences();
            DisposeTimer();
        }
    }

    private void DisposeTimer()
    {
        _jobTimer?.Dispose();
        _jobTimer = null;
    }

    public void StopJob()
    {
        DisposeTimer();
    }

    protected abstract void ExecuteJob();
}