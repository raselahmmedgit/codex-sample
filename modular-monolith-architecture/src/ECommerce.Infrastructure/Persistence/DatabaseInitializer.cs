using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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

        await using var scope = services.CreateAsyncScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
        if (settings.ApplyMigrationsOnStartup) await dbContext.Database.MigrateAsync(cancellationToken);
        if (settings.SeedDevelopmentData)
        {
            await SeedRolesAsync(scopedServices, cancellationToken);
            await SeedDevelopmentDataAsync(dbContext, cancellationToken);
        }
    }

    private static async Task SeedRolesAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        foreach (var roleName in new[] { "Admin", "Customer", "Manager", "Seller" })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new ApplicationRole(roleName));
        }
    }

    private static async Task SeedDevelopmentDataAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var category = new Category("Electronics");
        if (!await dbContext.Categories.AnyAsync(x => x.Name == category.Name, cancellationToken))
            await dbContext.Categories.AddAsync(category, cancellationToken);

        var brand = new Brand("Elevate");
        if (!await dbContext.Brands.AnyAsync(x => x.Name == brand.Name, cancellationToken))
            await dbContext.Brands.AddAsync(brand, cancellationToken);

        var products = new[]
        {
            new ProductSeed("ELV-AUDIO-001", "Elevate Wireless Headphones", "Comfortable noise-isolating headphones for focused work and relaxed listening.", 129.99m),
            new ProductSeed("ELV-DESK-002", "Minimal Desk Lamp", "A warm, adjustable desk lamp designed for calm evening workspaces.", 64.50m),
            new ProductSeed("ELV-TRVL-003", "Everyday Carry Backpack", "A durable everyday backpack with a padded laptop sleeve and smart storage.", 89m),
            new ProductSeed("ELV-HOME-004", "Stoneware Coffee Mug", "Hand-finished stoneware mug with a comfortable matte grip.", 24.95m),
            new ProductSeed("ELV-OFFICE-005", "Cloud Notes Notebook", "Premium dotted pages for planning, sketching and daily notes.", 18.75m),
            new ProductSeed("ELV-TRVL-006", "Weekender Travel Tote", "A lightweight carryall for short trips, gym days and busy commutes.", 72m),
            new ProductSeed("ELV-HOME-007", "Linen Throw Blanket", "A soft textured throw that adds warmth and character to any room.", 58.25m),
            new ProductSeed("ELV-OFFICE-008", "Focus Timer", "A simple tactile timer for distraction-free work sessions.", 31.40m)
        };

        foreach (var seed in products)
        {
            if (await dbContext.Products.AnyAsync(x => x.Sku == seed.Sku, cancellationToken)) continue;
            var product = new Product(seed.Sku, seed.Name, seed.Price);
            product.Update(seed.Name, seed.Description, seed.Price);
            product.ChangeStatus(Domain.Enums.ProductStatus.Active);
            await dbContext.Products.AddAsync(product, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private sealed record ProductSeed(string Sku, string Name, string Description, decimal Price);
}
