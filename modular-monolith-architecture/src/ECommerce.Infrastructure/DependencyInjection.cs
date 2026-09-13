using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Logging;
using ECommerce.Application.Features.Auth;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Authentication;
using ECommerce.Infrastructure.Configuration;
using ECommerce.Infrastructure.Logging;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Payments;
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
        services.AddOptions<JwtSettings>().BindConfiguration(JwtSettings.SectionName).ValidateOnStart();
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, MockPaymentService>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        return services;
    }
}
