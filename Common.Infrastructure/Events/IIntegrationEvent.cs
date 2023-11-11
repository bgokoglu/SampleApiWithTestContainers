using MediatR;
using MediatR.ParallelPublisher;

namespace Common.Infrastructure.Events;

public interface IIntegrationEvent : IFireAndForgetNotification
{
    Guid Id { get; }
    DateTimeOffset OccurredDateTime { get; }
}