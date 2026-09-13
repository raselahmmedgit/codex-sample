using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ECOMMERCE_DESIGN_CONNECTION")
            ?? "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True;";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;
        return new ApplicationDbContext(options);
    }
}
