using CronScheduler.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CronScheduler.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCronScheduler(this IServiceCollection services)
    {
        services.AddSingleton<ICronScheduler, CronScheduler>();
        return services;
    }
}