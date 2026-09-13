namespace ECommerce.Infrastructure.Configuration;

public sealed class DatabaseSettings
{
    public const string SectionName = "Database";
    public string Provider { get; init; } = "SqlServer";
    public string DefaultConnectionName { get; init; } = "DefaultConnection";
    public bool ApplyMigrationsOnStartup { get; init; }
    public bool SeedDevelopmentData { get; init; }
}
