using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Products.Models;

public sealed record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    decimal Price,
    ProductStatus Status);
