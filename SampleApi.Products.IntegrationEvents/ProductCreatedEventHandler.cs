using Microsoft.Extensions.Logging;
using SampleApi.Common.Infrastructure.Events;

namespace SampleApi.Products.IntegrationEvents;

internal sealed class ProductCreatedEventHandler : IIntegrationEventHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ProductCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ProductCreatedEvent for productId: {Val} is being handled", @event.ProductId);
        
        await Task.Delay(2000, cancellationToken);
        
        _logger.LogInformation("ProductCreatedEvent for productId: {Val} has been handled", @event.ProductId);
    }
}
