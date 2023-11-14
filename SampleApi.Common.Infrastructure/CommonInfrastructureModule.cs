using SampleApi.Common.Infrastructure.Events.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using SampleApi.Common.Core.Repository;

namespace SampleApi.Common.Infrastructure;

public static class CommonInfrastructureModule
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services)
    {
        services.AddEventBus();
        services.AddFeatureManagement();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
