using Common.Infrastructure.Events;

namespace SampleApiWithTestContainers.Products;

public record ProductCreatedEvent
    (Guid Id, Guid ProductId, DateTimeOffset OccurredDateTime) : IIntegrationEvent
{
    public static ProductCreatedEvent Create(Guid productId) =>
        new(Guid.NewGuid(), productId, DateTimeOffset.Now);
}