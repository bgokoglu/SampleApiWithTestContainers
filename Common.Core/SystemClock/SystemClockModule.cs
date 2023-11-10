using Microsoft.Extensions.DependencyInjection;

namespace Common.Core.SystemClock;

public static class SystemClockModule
{
    public static IServiceCollection AddSystemClock(this IServiceCollection services)
    {
        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}