using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ECOMMERCE_DESIGN_CONNECTION")
            ?? "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True;";
        var provider = Environment.GetEnvironmentVariable("ECOMMERCE_DESIGN_PROVIDER") ?? "SqlServer";
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        if (provider.Equals("PostgreSql", StringComparison.OrdinalIgnoreCase) || provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
            optionsBuilder.UseNpgsql(connectionString);
        else if (provider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        else if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            optionsBuilder.UseSqlServer(connectionString);
        else
            throw new InvalidOperationException($"Unsupported design-time database provider '{provider}'.");

        var options = optionsBuilder.Options;
        return new ApplicationDbContext(options);
    }
}
