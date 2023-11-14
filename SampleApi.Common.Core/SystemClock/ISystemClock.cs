namespace SampleApi.Common.Core.SystemClock;

public interface ISystemClock
{
    DateTimeOffset Now { get; }
}