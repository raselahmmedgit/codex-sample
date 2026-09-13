using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products.Models;

namespace ECommerce.Application.Features.Products.Services;

public interface IProductService
{
    Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> SearchAsync(ProductFilterRequest request, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ProductDto>> ChangeStatusAsync(Guid id, ChangeProductStatusRequest request, CancellationToken cancellationToken = default);
}
