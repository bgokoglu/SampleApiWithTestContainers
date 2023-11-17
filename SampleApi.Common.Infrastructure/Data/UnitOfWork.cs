using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SampleApi.Common.Infrastructure.Events;
using SampleApi.Common.Infrastructure.Events.EventBus;

namespace SampleApi.Common.Infrastructure.Data;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IEventBus _eventBus;
    private IDbContextTransaction? _transaction;
    private readonly List<IIntegrationEvent> _events;

    public UnitOfWork(TContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
        _events = new List<IIntegrationEvent>();
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if(_transaction is not null)
            await _transaction.CommitAsync(cancellationToken);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        int result;

        if (_events.Count == 0)
        {
            result = await _context.SaveChangesAsync(cancellationToken);
            
            if(_transaction is not null)
                await CommitTransactionAsync(cancellationToken);
        }
        else
        {
            if(_transaction is null)
                await BeginTransactionAsync(cancellationToken);
            
            result = await _context.SaveChangesAsync(cancellationToken);
            PublishEvents();
            await CommitTransactionAsync(cancellationToken);
        }

        return result;
    }

    public void AddEvent(IIntegrationEvent integrationEvent)
    {
        _events.Add(integrationEvent);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        _events.Clear();
        
        GC.SuppressFinalize(this);
    }

    private void PublishEvents()
    {
        foreach (var integrationEvent in _events)
        {
            _eventBus.PublishAsync(integrationEvent);
        }

        _events.Clear();
    }
}