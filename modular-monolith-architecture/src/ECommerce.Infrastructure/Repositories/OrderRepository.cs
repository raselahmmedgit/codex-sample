using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => dbContext.Orders.Include(x => x.Items).Include(x => x.StatusHistory).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    public async Task<IReadOnlyCollection<Order>> ListByUserAsync(Guid userId, CancellationToken cancellationToken = default) => await dbContext.Orders.AsNoTracking().Include(x => x.Items).Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAtUtc).ToArrayAsync(cancellationToken);
    public Task AddAsync(Order order, CancellationToken cancellationToken = default) => dbContext.Orders.AddAsync(order, cancellationToken).AsTask();
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}
