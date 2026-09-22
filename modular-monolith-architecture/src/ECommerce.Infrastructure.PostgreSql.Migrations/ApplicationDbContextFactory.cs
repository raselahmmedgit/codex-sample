using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.Infrastructure.PostgreSql.Migrations;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ECOMMERCE_DESIGN_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=ECommerceDb;Username=postgres";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString, postgres => postgres.MigrationsAssembly(typeof(ApplicationDbContextFactory).Assembly.GetName().Name))
            .Options;

        return new ApplicationDbContext(options);
    }
}
