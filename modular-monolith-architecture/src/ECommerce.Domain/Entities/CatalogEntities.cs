using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Product : AuditableEntity
{
    private readonly List<ProductImage> _images = [];
    private readonly List<ProductCategory> _categories = [];

    private Product() { }

    public Product(string sku, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(sku)) throw new DomainException("Product SKU is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Product name is required.");
        if (price < 0) throw new DomainException("Product price cannot be negative.");
        Sku = sku.Trim();
        Name = name.Trim();
        Price = price;
    }

    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public ProductStatus Status { get; private set; } = ProductStatus.Draft;
    public IReadOnlyCollection<ProductImage> Images => _images;
    public IReadOnlyCollection<ProductCategory> Categories => _categories;

    public void Update(string name, string? description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Product name is required.");
        if (price < 0) throw new DomainException("Product price cannot be negative.");
        Name = name.Trim();
        Description = description?.Trim();
        Price = price;
        MarkUpdated();
    }

    public void ChangeStatus(ProductStatus status)
    {
        if (Status == ProductStatus.Archived && status != ProductStatus.Archived)
            throw new DomainException("An archived product cannot be reactivated.");
        Status = status;
        MarkUpdated();
    }
}

public sealed class ProductImage : BaseEntity
{
    private ProductImage() { }
    public ProductImage(Guid productId, string storageKey, string contentType)
    {
        if (productId == Guid.Empty) throw new DomainException("Product ID is required.");
        if (string.IsNullOrWhiteSpace(storageKey)) throw new DomainException("Image storage key is required.");
        ProductId = productId;
        StorageKey = storageKey;
        ContentType = contentType;
    }

    public Guid ProductId { get; private set; }
    public string StorageKey { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; }

    public void MarkAsPrimary() => IsPrimary = true;
}

public sealed class Category : AuditableEntity
{
    private Category() { }
    public Category(string name, Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Category name is required.");
        Name = name.Trim();
        ParentCategoryId = parentCategoryId;
    }

    public string Name { get; private set; } = string.Empty;
    public Guid? ParentCategoryId { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int SortOrder { get; private set; }
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}

public sealed class Brand : AuditableEntity
{
    private Brand() { }
    public Brand(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Brand name is required.");
        Name = name.Trim();
    }

    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}

public sealed class ProductCategory : BaseEntity
{
    private ProductCategory() { }
    public ProductCategory(Guid productId, Guid categoryId)
    {
        ProductId = productId;
        CategoryId = categoryId;
    }

    public Guid ProductId { get; private set; }
    public Guid CategoryId { get; private set; }
}
