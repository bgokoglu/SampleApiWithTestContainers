using DotNetCore.CAP;
using MediatR;

namespace SampleApi.Common.Infrastructure.Events;

public interface IIntegrationEventHandler<in TEvent> : INotificationHandler<TEvent>, ICapSubscribe
    where TEvent : IIntegrationEvent
{
}