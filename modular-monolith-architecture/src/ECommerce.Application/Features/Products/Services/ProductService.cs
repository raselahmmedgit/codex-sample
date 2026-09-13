using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Products.Models;

namespace ECommerce.Application.Features.Products.Services;

public sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        return product is null
            ? Result<ProductDto>.Failure("Product was not found.")
            : Result<ProductDto>.Success(Map(product), "Product retrieved successfully.");
    }

    public async Task<Result<PagedResult<ProductDto>>> SearchAsync(ProductFilterRequest request, CancellationToken cancellationToken = default)
    {
        request.Validate();
        var products = await productRepository.SearchAsync(request.Search, request.PageNumber, request.PageSize, cancellationToken);
        var result = new PagedResult<ProductDto>(products.Items.Select(Map).ToArray(), products.PageNumber, products.PageSize, products.TotalCount);
        return Result<PagedResult<ProductDto>>.Success(result, "Products retrieved successfully.");
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Domain.Entities.Product(request.Sku, request.Name, request.Price);
        product.Update(request.Name, request.Description, request.Price);
        await productRepository.AddAsync(product, cancellationToken); await productRepository.SaveChangesAsync(cancellationToken);
        return Result<ProductDto>.Success(Map(product), "Product created successfully.");
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null) return Result<ProductDto>.Failure("Product was not found.");
        product.Update(request.Name, request.Description, request.Price); productRepository.Update(product);
        await productRepository.SaveChangesAsync(cancellationToken);
        return Result<ProductDto>.Success(Map(product), "Product updated successfully.");
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null) return Result<bool>.Failure("Product was not found.");
        productRepository.Remove(product); await productRepository.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true, "Product deleted successfully.");
    }

    public async Task<Result<ProductDto>> ChangeStatusAsync(Guid id, ChangeProductStatusRequest request, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);
        if (product is null) return Result<ProductDto>.Failure("Product was not found.");
        product.ChangeStatus(request.Status); productRepository.Update(product);
        await productRepository.SaveChangesAsync(cancellationToken);
        return Result<ProductDto>.Success(Map(product), "Product status updated successfully.");
    }

    private static ProductDto Map(Domain.Entities.Product product) =>
        new(product.Id, product.Sku, product.Name, product.Description, product.Price, product.Status);
}
