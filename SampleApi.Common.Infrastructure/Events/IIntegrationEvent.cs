using MediatR.ParallelPublisher;

namespace SampleApi.Common.Infrastructure.Events;

public interface IIntegrationEvent : IFireAndForgetNotification
{
    Guid Id { get; }
    DateTimeOffset OccurredDateTime { get; }
}