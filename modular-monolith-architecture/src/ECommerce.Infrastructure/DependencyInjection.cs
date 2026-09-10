using ECommerce.Application.Common.Logging;
using ECommerce.Infrastructure.Configuration;
using ECommerce.Infrastructure.Logging;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>() ?? new();
        var connectionString = configuration.GetConnectionString(databaseSettings.DefaultConnectionName)
            ?? throw new InvalidOperationException("The DefaultConnection connection string is missing.");

        services.AddOptions<DatabaseSettings>().BindConfiguration(DatabaseSettings.SectionName).ValidateOnStart();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (databaseSettings.Provider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                return;
            }

            if (!databaseSettings.Provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Unsupported database provider '{databaseSettings.Provider}'.");

            options.UseSqlServer(connectionString);
        });

        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        return services;
    }
}
