using MediatR.ParallelPublisher;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Products.IntegrationEvents;

namespace SampleApi.Products.Infrastructure.Mediator;

internal static class MediationModule
{
    internal static IServiceCollection AddMediationModule(this IServiceCollection services)
    {
        var eventHandlersAssembly = typeof(ProductCreatedEvent).Assembly;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(eventHandlersAssembly);
            configuration.UseParallelNotificationPublisher(services);
        });

        return services;
    }
}