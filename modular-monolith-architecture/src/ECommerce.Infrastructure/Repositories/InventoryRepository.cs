using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class InventoryRepository(ApplicationDbContext dbContext) : IInventoryRepository
{
    public Task<Inventory?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default) => dbContext.Inventories.SingleOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
    public Task AddAsync(Inventory inventory, CancellationToken cancellationToken = default) => dbContext.Inventories.AddAsync(inventory, cancellationToken).AsTask();
    public Task AddTransactionAsync(InventoryTransaction transaction, CancellationToken cancellationToken = default) => dbContext.InventoryTransactions.AddAsync(transaction, cancellationToken).AsTask();
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}
