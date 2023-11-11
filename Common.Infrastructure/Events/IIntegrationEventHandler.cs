using MediatR;

namespace Common.Infrastructure.Events;

public interface IIntegrationEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IIntegrationEvent
{
}