using Common.Infrastructure.Events.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Common.Infrastructure;

public static class CommonInfrastructureModule
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services)
    {
        services.AddEventBus();
        services.AddFeatureManagement();

        return services;
    }
}
