using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Catalog.Models;

namespace ECommerce.Application.Features.Catalog;

public interface ICatalogService
{
    Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result<CategoryDto>> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyCollection<CategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<BrandDto>> CreateBrandAsync(CreateBrandRequest request, CancellationToken cancellationToken = default);
    Task<Result<BrandDto>> UpdateBrandAsync(Guid id, UpdateBrandRequest request, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyCollection<BrandDto>>> GetBrandsAsync(CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteBrandAsync(Guid id, CancellationToken cancellationToken = default);
}
