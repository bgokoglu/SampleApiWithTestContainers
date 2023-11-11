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

    private const int SlowQueryThreshold = 100; //milliseconds
    
    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        if (eventData.Duration.TotalMilliseconds > SlowQueryThreshold)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<SlowQueryInterceptor>>();
            logger.LogWarning("Slow query {DurationTotalMilliseconds} ms: {CommandText}", eventData.Duration.TotalMilliseconds, command.CommandText);
        }

        return base.ReaderExecuted(command, eventData, result);
    }
}