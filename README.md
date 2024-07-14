[![.NET](https://github.com/tiger4589/CronScheduler/actions/workflows/dotnet.yml/badge.svg)](https://github.com/tiger4589/CronScheduler/actions/workflows/dotnet.yml)

# Cron Expression Scheduler for .Net

A .Net scheduler using cron expressions built on top of [NCrontab by atifaziz](https://github.com/atifaziz/NCrontab)

## How to install?

You can find the library on nuget.org by searching for Crontab.Scheduler

## How to inject the scheduler?

You have to call the `.AddCronScheduler()` on your service collection.
Once done, you can directly inject the `ICronScheduler` interface in your desired class to use it.

Example:

```csharp
IServiceProvider BuildServiceProvider() => new ServiceCollection()
    .AddCronScheduler()
    .BuildServiceProvider();
```

## How to use?

In general, you have to call the `ScheduleJob` method using the instance of `ICronScheduler` you injected.
This method takes only one argument of type `ISchedulerOptions`.
The library comes with a builder to make it easier to prepare this parameter.

Current options included:

- CronExpression - required.
- StartDate - optional - Now by default
- EndDate - optional - Now + 1 year by default
- NumberOfTimesToExecute - optional - null by default
- One of the two next options is required, and can't have both:
    - CronExecutor 
    - ActionToExecute

### SchedulerOptionsBuilder

The builder is straightforward to use.
For example:

```csharp
new SchedulerOptionsBuilder("* * * * *")
    .WithEndDate(DateTime.Now.AddMinutes(2)) 
    .WithActionToExecute(() => Console.WriteLine($"{DateTime.Now}: Hello from an action!"))
    .WithNumberOfTimesToExecute(3)
    .Build()
```

### Cron expressions format:

**Five-part format without seconds**:

    * * * * *
    - - - - -
    | | | | |
    | | | | +----- day of week (0 - 6) (Sunday=0)
    | | | +------- month (1 - 12)
    | | +--------- day of month (1 - 31)
    | +----------- hour (0 - 23)
    +------------- min (0 - 59)

**or a six-part format that allows for seconds**:

    * * * * * *
    - - - - - -
    | | | | | |
    | | | | | +--- day of week (0 - 6) (Sunday=0)
    | | | | +----- month (1 - 12)
    | | | +------- day of month (1 - 31)
    | | +--------- hour (0 - 23)
    | +----------- min (0 - 59)
    +------------- sec (0 - 59)

### Start and End date:

When specified, the scheduler will only execute your desired function between these two dates.

### Number of times to execute:

Regardless of how many occurences the scheduler finds between start and end date, it will only execute the desired function as much as specified.

### Cron Executor

To use an executor, you have to implement the interface `ICronExecutor` and implement the function `Execute`.
For example:

```csharp
public class MyExecutor : ICronExecutor
{
    public void Execute()
    {
        Console.WriteLine($"{DateTime.Now}: Hello From Executor!");
    }
}
```

Then you pass an instance of this executor to the builder using the function `WithCronExecutor(executor)`.

### Action to execute

If you want a quick action to be executed without the need to implement the interface, you can use an action such as: `.WithActionToExecute(() => Console.WriteLine($"{DateTime.Now}: Hello from an action!"))`

## How to follow executions?

When you call `ScheduleJob`, an instance of `ICronJob` is returned (if successful).
Four events can be listened to:

- OnJobExecuted - Fires after an action or an executor being successfully executed.
- OnJobNoMoreOccurrences - Fires when the scheduler can no longer find any occurrences between start and end date.
- OnJobMaxExecution - Fires when the scheduler reaches the maximum number to execute (if provided)
- OnErrorExecuting - Fires when an exception is caught during executing.

