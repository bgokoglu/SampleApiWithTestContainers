using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savorboard.CAP.InMemoryMessageQueue;

namespace SampleApi.Common.Infrastructure.Events.EventBus.Cap;

public static class CapEventBusModule
{
    public static IServiceCollection AddCapEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(x =>
        {
            x.UsePostgreSql(configuration.GetConnectionString("DefaultConnection")!);
            x.UseInMemoryMessageQueue();
            x.UseDashboard();
        });

        return services;
    }
}