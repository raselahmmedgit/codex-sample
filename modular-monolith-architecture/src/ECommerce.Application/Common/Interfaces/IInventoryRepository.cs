using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Interfaces;

public interface IInventoryRepository
{
    Task<Inventory?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(Inventory inventory, CancellationToken cancellationToken = default);
    Task AddTransactionAsync(InventoryTransaction transaction, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
