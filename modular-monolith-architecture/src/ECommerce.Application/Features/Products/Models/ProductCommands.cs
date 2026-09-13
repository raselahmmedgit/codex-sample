namespace ECommerce.Application.Features.Products.Models;

public sealed record CreateProductRequest(string Sku, string Name, string? Description, decimal Price);
public sealed record UpdateProductRequest(string Name, string? Description, decimal Price);
public sealed record ChangeProductStatusRequest(ECommerce.Domain.Enums.ProductStatus Status);
