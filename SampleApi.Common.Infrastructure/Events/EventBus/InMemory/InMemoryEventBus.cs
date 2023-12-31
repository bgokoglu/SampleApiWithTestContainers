using MediatR;
using Microsoft.Extensions.Logging;

namespace SampleApi.Common.Infrastructure.Events.EventBus.InMemory;

internal sealed class InMemoryEventBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBus(IMediator mediator, ILogger<InMemoryEventBus> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IIntegrationEvent
    {
        return _mediator.Publish(@event, cancellationToken);
    }
}
