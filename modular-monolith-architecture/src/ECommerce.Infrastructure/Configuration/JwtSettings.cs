namespace ECommerce.Infrastructure.Configuration;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = "ECommerce.API";
    public string Audience { get; init; } = "ECommerce.Client";
    public string SecretKey { get; init; } = string.Empty;
    public int AccessTokenMinutes { get; init; } = 15;
    public int RefreshTokenDays { get; init; } = 7;
}
