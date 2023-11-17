using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using SampleApi.Common.Infrastructure.Events;

namespace SampleApi.Products.IntegrationEvents;

public sealed class ProductCreatedEventHandler : IIntegrationEventHandler<ProductCreatedEvent>
{
    private const string EventName = "ProductCreatedEvent";
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    [CapSubscribe(EventName)]
    public async Task Handle(ProductCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{EventName} for ProductId: {ProductId} is being handled", EventName, @event.ProductId);
        _logger.LogInformation("{EventName}: {Body}", EventName, @event.ToString());
        
        await Task.Delay(2000, cancellationToken);
        
        _logger.LogInformation("{EventName} for ProductId: {ProductId} has been handled", EventName, @event.ProductId);
    }
}
