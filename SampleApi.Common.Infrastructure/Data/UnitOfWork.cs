using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SampleApi.Common.Infrastructure.Events;
using SampleApi.Common.Infrastructure.Events.EventBus;
using SampleApi.Common.Infrastructure.Events.EventBus.Cap;

namespace SampleApi.Common.Infrastructure.Data;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IEventBus _eventBus;
    private readonly ICapPublisher _capPublisher;
    private IDbContextTransaction? _transaction;
    private readonly List<IIntegrationEvent> _events;
    private readonly ILogger<UnitOfWork<TContext>> _logger;

    public UnitOfWork(TContext context, IEventBus eventBus, ICapPublisher capPublisher, ILogger<UnitOfWork<TContext>> logger)
    {
        _context = context;
        _eventBus = eventBus;
        _capPublisher = capPublisher;
        _logger = logger;
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
            await PublishEvents();
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

    private async Task PublishEvents()
    {
        foreach (var integrationEvent in _events)
        {
            _logger.LogInformation("Publishing event: {Name}", nameof(integrationEvent));
            await _eventBus.PublishAsync(integrationEvent);
            await _capPublisher.PublishAsync(CapEventBusEventName.ProductCreatedEvent, integrationEvent);
            _logger.LogInformation("Published event: {Name}", nameof(integrationEvent));
        }

        _events.Clear();
    }
}
