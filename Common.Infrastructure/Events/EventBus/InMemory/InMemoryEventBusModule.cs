using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Events.EventBus.InMemory;

public static class InMemoryEventBusModule
{
    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, InMemoryEventBus>();

        return services;
    }
}
