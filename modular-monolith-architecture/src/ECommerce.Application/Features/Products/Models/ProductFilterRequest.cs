using ECommerce.Application.Common.Models;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Products.Models;

public sealed record ProductFilterRequest : PaginationRequest
{
    public string? Search { get; init; }
    public Guid? CategoryId { get; init; }
    public Guid? BrandId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public ProductStatus? Status { get; init; }
    public bool? IsInStock { get; init; }
    public string SortBy { get; init; } = "Name";
    public bool SortDescending { get; init; }
}
