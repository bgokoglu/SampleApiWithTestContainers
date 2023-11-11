using System.Reflection;
//using MediatR.NotificationPublishers;
using MediatR.ParallelPublisher;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Mediator;

public static class MediatorModule
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
            //configuration.NotificationPublisher = new TaskWhenAllPublisher();
            configuration.UseParallelNotificationPublisher(services);
        });

        return services;
    }
}
