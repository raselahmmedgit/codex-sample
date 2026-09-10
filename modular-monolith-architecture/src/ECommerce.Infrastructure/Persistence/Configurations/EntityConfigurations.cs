using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.DisplayName).HasMaxLength(150);
    }
}

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens"); builder.HasKey(x => x.Id);
        builder.Property(x => x.TokenHash).HasMaxLength(512).IsRequired();
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.ExpiresAtUtc });
    }
}

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Sku).HasMaxLength(64).IsRequired();
        builder.HasIndex(x => x.Sku).IsUnique(); builder.HasIndex(x => x.Name);
        builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Price).HasPrecision(18, 2);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        builder.HasMany(x => x.Images).WithOne().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Categories).WithOne().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages"); builder.HasKey(x => x.Id);
        builder.Property(x => x.StorageKey).HasMaxLength(500).IsRequired();
        builder.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.IsPrimary });
    }
}

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.HasIndex(x => new { x.ParentCategoryId, x.Name }).IsUnique();
    }
}

internal sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
    }
}

internal sealed class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories"); builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.ProductId, x.CategoryId }).IsUnique();
    }
}

internal sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts"); builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasMany(x => x.Items).WithOne().HasForeignKey("CartId").OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems"); builder.HasKey(x => x.Id);
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
        builder.HasIndex("CartId", nameof(CartItem.ProductId)).IsUnique();
    }
}

internal sealed class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists"); builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasMany(x => x.Items).WithOne().HasForeignKey("WishlistId").OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
{
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        builder.ToTable("WishlistItems"); builder.HasKey(x => x.Id);
        builder.HasIndex("WishlistId", nameof(WishlistItem.ProductId)).IsUnique();
    }
}

internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Line1).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Line2).HasMaxLength(250);
        builder.Property(x => x.City).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Country).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => new { x.UserId, x.IsDefault });
    }
}

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders"); builder.HasKey(x => x.Id);
        builder.Property(x => x.OrderNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.OrderNumber).IsUnique();
        builder.HasIndex(x => new { x.UserId, x.Status, x.CreatedAtUtc });
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Subtotal).HasPrecision(18, 2);
        builder.Property(x => x.Discount).HasPrecision(18, 2);
        builder.Property(x => x.ShippingCost).HasPrecision(18, 2);
        builder.Ignore(x => x.Total);
        builder.HasMany(x => x.Items).WithOne().HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.StatusHistory).WithOne().HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade);
    }
}

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems"); builder.HasKey(x => x.Id);
        builder.Property(x => x.ProductName).HasMaxLength(250).IsRequired();
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2); builder.Ignore(x => x.Total);
    }
}

internal sealed class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.ToTable("OrderStatusHistories"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Reason).HasMaxLength(500).IsRequired();
        builder.HasIndex("OrderId", nameof(OrderStatusHistory.ChangedAtUtc));
    }
}

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Provider).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => x.OrderId).IsUnique();
    }
}

internal sealed class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Reference).HasMaxLength(150).IsRequired();
        builder.HasIndex(x => x.Reference).IsUnique();
    }
}

internal sealed class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventories"); builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ProductId).IsUnique(); builder.Ignore(x => x.AvailableQuantity);
    }
}

internal sealed class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("InventoryTransactions"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Reason).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => new { x.InventoryId, x.CreatedAtUtc });
    }
}

internal sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired(); builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Value).HasPrecision(18, 2); builder.Property(x => x.MinimumOrderAmount).HasPrecision(18, 2);
        builder.Property(x => x.MaximumDiscount).HasPrecision(18, 2);
    }
}

internal sealed class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
{
    public void Configure(EntityTypeBuilder<CouponUsage> builder)
    {
        builder.ToTable("CouponUsages"); builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CouponId, x.UserId, x.OrderId }).IsUnique();
    }
}

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Comment).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.ModerationStatus).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.ProductId, x.UserId }).IsUnique(); builder.HasIndex(x => x.ProductId);
    }
}

internal sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Action).HasMaxLength(150).IsRequired(); builder.Property(x => x.EntityName).HasMaxLength(150).IsRequired();
        builder.HasIndex(x => new { x.EntityName, x.EntityId, x.CreatedAtUtc });
    }
}
