using SampleApi.Common.Infrastructure.Events.EventBus.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApi.Common.Infrastructure.Events.EventBus;

internal static class EventBusModule
{
    internal static IServiceCollection AddEventBus(this IServiceCollection services) =>
        services.AddInMemoryEventBus();
}
