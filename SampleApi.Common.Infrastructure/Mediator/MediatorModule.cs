using System.Reflection;
using MediatR.ParallelPublisher;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApi.Common.Infrastructure.Mediator;

public static class MediatorModule
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
            configuration.UseParallelNotificationPublisher(services);
        });

        return services;
    }
}
