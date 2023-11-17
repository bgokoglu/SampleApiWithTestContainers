using System.Text.Json;
using SampleApi.Common.Infrastructure.Events;

namespace SampleApi.Products.IntegrationEvents;

public record ProductCreatedEvent
    (Guid Id, Guid ProductId, DateTimeOffset OccurredDateTime) : IIntegrationEvent
{
    public static ProductCreatedEvent Create(Guid productId) =>
        new(Guid.NewGuid(), productId, DateTimeOffset.Now);

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}