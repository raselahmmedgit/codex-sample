using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration, bool isDevelopment, CancellationToken cancellationToken = default)
    {
        var settings = configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>() ?? new();
        if (!settings.ApplyMigrationsOnStartup && !settings.SeedDevelopmentData) return;
        if (settings.SeedDevelopmentData && !isDevelopment)
            throw new InvalidOperationException("SeedDevelopmentData can only be enabled in the Development environment.");

        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        if (settings.ApplyMigrationsOnStartup) await dbContext.Database.MigrateAsync(cancellationToken);
        if (settings.SeedDevelopmentData) await SeedDevelopmentDataAsync(dbContext, cancellationToken);
    }

    private static async Task SeedDevelopmentDataAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Products.AnyAsync(cancellationToken)) return;

        var category = new Category("Electronics");
        var brand = new Brand("Contoso");
        var product = new Product("LAPTOP-001", "Contoso Laptop", 999.99m);
        product.ChangeStatus(Domain.Enums.ProductStatus.Active);
        await dbContext.Categories.AddAsync(category, cancellationToken);
        await dbContext.Brands.AddAsync(brand, cancellationToken);
        await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
