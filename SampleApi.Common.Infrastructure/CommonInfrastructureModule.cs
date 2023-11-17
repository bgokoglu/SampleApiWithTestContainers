using Microsoft.Extensions.Configuration;
using SampleApi.Common.Infrastructure.Events.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using SampleApi.Common.Infrastructure.Data.Repository;
using SampleApi.Common.Infrastructure.Events.EventBus.Cap;

namespace SampleApi.Common.Infrastructure;

public static class CommonInfrastructureModule
{
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEventBus(configuration);
        services.AddFeatureManagement();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));        
        
        return services;
    }
}
