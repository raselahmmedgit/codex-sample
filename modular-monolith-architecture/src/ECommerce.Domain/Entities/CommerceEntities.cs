using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class ApplicationUser : AuditableEntity
{
    private ApplicationUser() { }
    public ApplicationUser(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required.");
        Email = email.Trim().ToLowerInvariant();
    }

    public string Email { get; private set; } = string.Empty;
    public string? DisplayName { get; private set; }
}

public sealed class RefreshToken : BaseEntity
{
    private RefreshToken() { }
    public RefreshToken(Guid userId, string tokenHash, DateTime expiresAtUtc)
    {
        if (userId == Guid.Empty) throw new DomainException("User ID is required.");
        if (string.IsNullOrWhiteSpace(tokenHash)) throw new DomainException("Token hash is required.");
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
    }

    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public bool IsActive => RevokedAtUtc is null && ExpiresAtUtc > DateTime.UtcNow;
    public void Revoke() => RevokedAtUtc ??= DateTime.UtcNow;
}

public sealed class Cart : AuditableEntity
{
    private readonly List<CartItem> _items = [];
    private Cart() { }
    public Cart(Guid userId)
    {
        if (userId == Guid.Empty) throw new DomainException("User ID is required.");
        UserId = userId;
    }

    public Guid UserId { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items;

    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new DomainException("Cart quantity must be greater than zero.");
        if (unitPrice < 0) throw new DomainException("Cart item price cannot be negative.");
        var item = _items.SingleOrDefault(x => x.ProductId == productId);
        if (item is null) _items.Add(new CartItem(productId, quantity, unitPrice));
        else item.ChangeQuantity(item.Quantity + quantity, unitPrice);
        MarkUpdated();
    }
}

public sealed class CartItem : BaseEntity
{
    private CartItem() { }
    internal CartItem(Guid productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ChangeQuantity(quantity, unitPrice);
    }

    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => Quantity * UnitPrice;

    public void ChangeQuantity(int quantity, decimal serverSidePrice)
    {
        if (quantity <= 0) throw new DomainException("Cart quantity must be greater than zero.");
        if (serverSidePrice < 0) throw new DomainException("Cart item price cannot be negative.");
        Quantity = quantity;
        UnitPrice = serverSidePrice;
    }
}

public sealed class Wishlist : AuditableEntity
{
    private readonly List<WishlistItem> _items = [];
    private Wishlist() { }
    public Wishlist(Guid userId) { UserId = userId; }
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<WishlistItem> Items => _items;
    public void Add(Guid productId)
    {
        if (_items.All(x => x.ProductId != productId)) _items.Add(new WishlistItem(productId));
    }
}

public sealed class WishlistItem : BaseEntity
{
    private WishlistItem() { }
    internal WishlistItem(Guid productId) => ProductId = productId;
    public Guid ProductId { get; private set; }
}

public sealed class Address : AuditableEntity
{
    private Address() { }
    public Address(Guid userId, string line1, string city, string country)
    {
        UserId = userId;
        if (string.IsNullOrWhiteSpace(line1) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
            throw new DomainException("Address line, city, and country are required.");
        Line1 = line1.Trim();
        City = city.Trim();
        Country = country.Trim();
    }

    public Guid UserId { get; private set; }
    public string Line1 { get; private set; } = string.Empty;
    public string? Line2 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string? State { get; private set; }
    public string? PostalCode { get; private set; }
    public string Country { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }
    public void MarkAsDefault() => IsDefault = true;
}
