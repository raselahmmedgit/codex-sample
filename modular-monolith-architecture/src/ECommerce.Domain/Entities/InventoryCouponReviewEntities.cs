using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Inventory : AuditableEntity
{
    private Inventory() { }
    public Inventory(Guid productId, int quantity)
    {
        if (quantity < 0) throw new DomainException("Inventory quantity cannot be negative.");
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int AvailableQuantity => Quantity - ReservedQuantity;

    public void Reserve(int quantity)
    {
        if (quantity <= 0 || quantity > AvailableQuantity) throw new DomainException("Insufficient available inventory.");
        ReservedQuantity += quantity;
    }

    public void Release(int quantity)
    {
        if (quantity <= 0 || quantity > ReservedQuantity) throw new DomainException("Invalid inventory release quantity.");
        ReservedQuantity -= quantity;
    }

    public void Adjust(int quantity)
    {
        if (quantity < 0 || quantity < ReservedQuantity) throw new DomainException("Inventory cannot be less than reserved quantity.");
        Quantity = quantity;
        MarkUpdated();
    }
}

public sealed class InventoryTransaction : BaseEntity
{
    private InventoryTransaction() { }
    public InventoryTransaction(Guid inventoryId, int quantity, InventoryTransactionType type, string reason)
    {
        InventoryId = inventoryId;
        Quantity = quantity;
        Type = type;
        Reason = reason;
    }
    public Guid InventoryId { get; private set; }
    public int Quantity { get; private set; }
    public InventoryTransactionType Type { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
}

public sealed class Coupon : AuditableEntity
{
    private Coupon() { }
    public Coupon(string code, CouponType type, decimal value, DateTime startsAtUtc, DateTime expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new DomainException("Coupon code is required.");
        if (value <= 0) throw new DomainException("Coupon value must be greater than zero.");
        if (expiresAtUtc <= startsAtUtc) throw new DomainException("Coupon expiry must be after start date.");
        Code = code.Trim().ToUpperInvariant();
        Type = type;
        Value = value;
        StartsAtUtc = startsAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }
    public string Code { get; private set; } = string.Empty;
    public CouponType Type { get; private set; }
    public decimal Value { get; private set; }
    public decimal MinimumOrderAmount { get; private set; }
    public decimal? MaximumDiscount { get; private set; }
    public DateTime StartsAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public int? UsageLimit { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsValid(DateTime utcNow, decimal orderAmount) => IsActive && utcNow >= StartsAtUtc && utcNow <= ExpiresAtUtc && orderAmount >= MinimumOrderAmount;
    public decimal CalculateDiscount(decimal orderAmount)
    {
        if (!IsValid(DateTime.UtcNow, orderAmount)) return 0m;
        var discount = Type == CouponType.Percentage ? orderAmount * Value / 100m : Value;
        return Math.Min(discount, MaximumDiscount ?? discount);
    }
    public void Deactivate() => IsActive = false;
}

public sealed class CouponUsage : BaseEntity
{
    private CouponUsage() { }
    public CouponUsage(Guid couponId, Guid userId, Guid orderId)
    {
        CouponId = couponId;
        UserId = userId;
        OrderId = orderId;
    }
    public Guid CouponId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid OrderId { get; private set; }
    public DateTime UsedAtUtc { get; private set; } = DateTime.UtcNow;
}

public sealed class Review : AuditableEntity
{
    private Review() { }
    public Review(Guid productId, Guid userId, int rating, string comment)
    {
        if (rating is < 1 or > 5) throw new DomainException("Rating must be between 1 and 5.");
        ProductId = productId;
        UserId = userId;
        Rating = rating;
        Comment = comment.Trim();
    }
    public Guid ProductId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public ReviewModerationStatus ModerationStatus { get; private set; } = ReviewModerationStatus.Pending;
    public void Moderate(ReviewModerationStatus status) => ModerationStatus = status;
}

public sealed class AuditLog : BaseEntity
{
    private AuditLog() { }
    public AuditLog(Guid? userId, string action, string entityName, Guid? entityId)
    {
        UserId = userId;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
    }
    public Guid? UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string EntityName { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
}
