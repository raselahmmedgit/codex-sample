namespace ECommerce.Application.Features.Catalog.Models;

public sealed record CreateCategoryRequest(string Name, Guid? ParentCategoryId);
public sealed record UpdateCategoryRequest(string Name, bool IsActive);
public sealed record CategoryDto(Guid Id, string Name, Guid? ParentCategoryId, bool IsActive, int SortOrder);
public sealed record CreateBrandRequest(string Name);
public sealed record UpdateBrandRequest(string Name, bool IsActive);
public sealed record BrandDto(Guid Id, string Name, bool IsActive);
