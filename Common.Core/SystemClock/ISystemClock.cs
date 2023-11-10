namespace Common.Core.SystemClock;

public interface ISystemClock
{
    DateTimeOffset Now { get; }
}