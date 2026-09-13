using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class CartRepository(ApplicationDbContext dbContext) : ICartRepository
{
    public Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) => dbContext.Carts.Include(x => x.Items).SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    public Task AddAsync(Cart cart, CancellationToken cancellationToken = default) => dbContext.Carts.AddAsync(cart, cancellationToken).AsTask();
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}

public sealed class WishlistRepository(ApplicationDbContext dbContext) : IWishlistRepository
{
    public Task<Wishlist?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) => dbContext.Wishlists.Include(x => x.Items).SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    public Task AddAsync(Wishlist wishlist, CancellationToken cancellationToken = default) => dbContext.Wishlists.AddAsync(wishlist, cancellationToken).AsTask();
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}
