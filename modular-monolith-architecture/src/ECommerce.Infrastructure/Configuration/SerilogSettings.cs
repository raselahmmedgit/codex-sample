namespace ECommerce.Infrastructure.Configuration;

public sealed class SerilogSettings
{
    public const string SectionName = "SerilogSettings";
    public int SlowRequestThresholdMilliseconds { get; init; } = 1000;
    public int RetainedFileCountLimit { get; init; } = 14;
}
