using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Cart;

public sealed class CartService(ICartRepository cartRepository, IWishlistRepository wishlistRepository, IProductRepository productRepository)
{
    public async Task<Result<CartDto>> GetCartAsync(Guid userId, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdAsync(userId, cancellationToken) ?? new Domain.Entities.Cart(userId);
        return Result<CartDto>.Success(Map(cart));
    }

    public async Task<Result<CartDto>> AddItemAsync(Guid userId, AddCartItemRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null || product.Status != Domain.Enums.ProductStatus.Active) return Result<CartDto>.Failure("Product is not available.");
        var cart = await cartRepository.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null) { cart = new Domain.Entities.Cart(userId); await cartRepository.AddAsync(cart, cancellationToken); }
        cart.AddItem(product.Id, request.Quantity, product.Price); await cartRepository.SaveChangesAsync(cancellationToken);
        return Result<CartDto>.Success(Map(cart), "Cart item added successfully.");
    }

    public async Task<Result<CartDto>> UpdateItemAsync(Guid userId, Guid itemId, UpdateCartItemRequest request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null) return Result<CartDto>.Failure("Cart was not found.");
        var item = cart.Items.SingleOrDefault(x => x.Id == itemId);
        var product = item is null ? null : await productRepository.GetByIdAsync(item.ProductId, cancellationToken);
        if (product is null) return Result<CartDto>.Failure("Product is not available.");
        cart.UpdateItem(itemId, request.Quantity, product.Price); await cartRepository.SaveChangesAsync(cancellationToken);
        return Result<CartDto>.Success(Map(cart), "Cart item updated successfully.");
    }

    public async Task<Result<bool>> RemoveItemAsync(Guid userId, Guid itemId, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null) return Result<bool>.Failure("Cart was not found.");
        cart.RemoveItem(itemId); await cartRepository.SaveChangesAsync(cancellationToken); return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> ClearAsync(Guid userId, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null) return Result<bool>.Success(true);
        cart.Clear(); await cartRepository.SaveChangesAsync(cancellationToken); return Result<bool>.Success(true);
    }

    public async Task<Result<WishlistDto>> GetWishlistAsync(Guid userId, CancellationToken cancellationToken)
    {
        var wishlist = await wishlistRepository.GetByUserIdAsync(userId, cancellationToken) ?? new Wishlist(userId);
        return Result<WishlistDto>.Success(new(wishlist.Id, wishlist.Items.Select(x => new WishlistItemDto(x.Id, x.ProductId)).ToArray()));
    }

    public async Task<Result<WishlistDto>> AddWishlistItemAsync(Guid userId, Guid productId, CancellationToken cancellationToken)
    {
        if (await productRepository.GetByIdAsync(productId, cancellationToken) is null) return Result<WishlistDto>.Failure("Product was not found.");
        var wishlist = await wishlistRepository.GetByUserIdAsync(userId, cancellationToken);
        if (wishlist is null) { wishlist = new Wishlist(userId); await wishlistRepository.AddAsync(wishlist, cancellationToken); }
        wishlist.Add(productId); await wishlistRepository.SaveChangesAsync(cancellationToken);
        return await GetWishlistAsync(userId, cancellationToken);
    }

    public async Task<Result<bool>> RemoveWishlistItemAsync(Guid userId, Guid itemId, CancellationToken cancellationToken)
    {
        var wishlist = await wishlistRepository.GetByUserIdAsync(userId, cancellationToken);
        if (wishlist is null) return Result<bool>.Failure("Wishlist was not found.");
        wishlist.Remove(itemId); await wishlistRepository.SaveChangesAsync(cancellationToken); return Result<bool>.Success(true);
    }

    private static CartDto Map(Domain.Entities.Cart cart) => new(cart.Id, cart.Items.Select(x => new CartItemDto(x.Id, x.ProductId, x.Quantity, x.UnitPrice, x.Total)).ToArray(), cart.Items.Sum(x => x.Total));
}
