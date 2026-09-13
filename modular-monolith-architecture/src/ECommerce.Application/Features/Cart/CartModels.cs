namespace ECommerce.Application.Features.Cart;

public sealed record AddCartItemRequest(Guid ProductId, int Quantity);
public sealed record UpdateCartItemRequest(int Quantity);
public sealed record CartItemDto(Guid Id, Guid ProductId, int Quantity, decimal UnitPrice, decimal Total);
public sealed record CartDto(Guid Id, IReadOnlyCollection<CartItemDto> Items, decimal Total);
public sealed record WishlistItemDto(Guid Id, Guid ProductId);
public sealed record WishlistDto(Guid Id, IReadOnlyCollection<WishlistItemDto> Items);
