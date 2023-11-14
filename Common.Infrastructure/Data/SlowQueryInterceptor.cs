using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure.Data;

public class SlowQueryInterceptor : DbCommandInterceptor
{
    private readonly IServiceProvider _serviceProvider;

    public SlowQueryInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private const int SlowQueryThreshold = 50; //milliseconds
    
    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Duration.TotalMilliseconds > SlowQueryThreshold)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<SlowQueryInterceptor>>();
            logger.LogWarning("*** Slow Query {DurationTotalMilliseconds} ms: {CommandText}", 
                eventData.Duration.TotalMilliseconds, command.CommandText);
        }
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }
}