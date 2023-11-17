using Microsoft.Extensions.Configuration;
using SampleApi.Common.Infrastructure.Events.EventBus.InMemory;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Common.Infrastructure.Events.EventBus.Cap;

namespace SampleApi.Common.Infrastructure.Events.EventBus;

internal static class EventBusModule
{
    internal static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        //for testing purposes, both are enabled
        services.AddInMemoryEventBus();
        services.AddCapEventBus(configuration);

        return services;
    }
}
