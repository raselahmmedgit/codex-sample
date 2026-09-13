using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Features.Catalog;
using ECommerce.Application.Features.Cart;
using ECommerce.Application.Features.Customers;
using ECommerce.Application.Features.Products.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<CartService>();
        services.AddScoped<CustomerService>();
        return services;
    }
}
