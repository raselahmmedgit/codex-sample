using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products.Models;

namespace ECommerce.Application.Features.Products.Services;

public interface IProductService
{
    Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProductDto>>> SearchAsync(ProductFilterRequest request, CancellationToken cancellationToken = default);
}
