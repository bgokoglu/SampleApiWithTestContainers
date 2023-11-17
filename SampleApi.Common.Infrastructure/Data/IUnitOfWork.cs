using Microsoft.EntityFrameworkCore;
using SampleApi.Common.Infrastructure.Events;

namespace SampleApi.Common.Infrastructure.Data;

public interface IUnitOfWork<TContext> : IDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    void AddEvent(IIntegrationEvent @event);
}