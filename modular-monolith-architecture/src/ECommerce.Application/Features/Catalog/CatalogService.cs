using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Catalog.Models;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Catalog;

public sealed class CatalogService(ICategoryRepository categoryRepository, IBrandRepository brandRepository) : ICatalogService
{
    public async Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Category(request.Name, request.ParentCategoryId);
        await categoryRepository.AddAsync(category, cancellationToken); await categoryRepository.SaveChangesAsync(cancellationToken);
        return Result<CategoryDto>.Success(Map(category), "Category created successfully.");
    }

    public async Task<Result<CategoryDto>> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return Result<CategoryDto>.Failure("Category was not found.");
        category.Rename(request.Name); if (request.IsActive) category.Activate(); else category.Deactivate();
        await categoryRepository.SaveChangesAsync(cancellationToken);
        return Result<CategoryDto>.Success(Map(category), "Category updated successfully.");
    }

    public async Task<Result<IReadOnlyCollection<CategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken = default) =>
        Result<IReadOnlyCollection<CategoryDto>>.Success((await categoryRepository.ListAsync(cancellationToken)).Select(Map).ToArray());

    public async Task<Result<bool>> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return Result<bool>.Failure("Category was not found.");
        categoryRepository.Remove(category); await categoryRepository.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true, "Category deleted successfully.");
    }

    public async Task<Result<BrandDto>> CreateBrandAsync(CreateBrandRequest request, CancellationToken cancellationToken = default)
    {
        var brand = new Brand(request.Name);
        await brandRepository.AddAsync(brand, cancellationToken); await brandRepository.SaveChangesAsync(cancellationToken);
        return Result<BrandDto>.Success(Map(brand), "Brand created successfully.");
    }

    public async Task<Result<BrandDto>> UpdateBrandAsync(Guid id, UpdateBrandRequest request, CancellationToken cancellationToken = default)
    {
        var brand = await brandRepository.GetByIdAsync(id, cancellationToken);
        if (brand is null) return Result<BrandDto>.Failure("Brand was not found.");
        brand.Rename(request.Name); if (request.IsActive) brand.Activate(); else brand.Deactivate();
        await brandRepository.SaveChangesAsync(cancellationToken);
        return Result<BrandDto>.Success(Map(brand), "Brand updated successfully.");
    }

    public async Task<Result<IReadOnlyCollection<BrandDto>>> GetBrandsAsync(CancellationToken cancellationToken = default) =>
        Result<IReadOnlyCollection<BrandDto>>.Success((await brandRepository.ListAsync(cancellationToken)).Select(Map).ToArray());

    public async Task<Result<bool>> DeleteBrandAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var brand = await brandRepository.GetByIdAsync(id, cancellationToken);
        if (brand is null) return Result<bool>.Failure("Brand was not found.");
        brandRepository.Remove(brand); await brandRepository.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true, "Brand deleted successfully.");
    }

    private static CategoryDto Map(Category x) => new(x.Id, x.Name, x.ParentCategoryId, x.IsActive, x.SortOrder);
    private static BrandDto Map(Brand x) => new(x.Id, x.Name, x.IsActive);
}
