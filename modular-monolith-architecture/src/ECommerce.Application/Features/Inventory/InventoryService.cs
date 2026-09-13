using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Inventory;

public sealed record AdjustInventoryRequest(Guid ProductId, int Quantity, string Reason);
public sealed record ReserveInventoryRequest(Guid ProductId, int Quantity, string Reason);
public sealed record InventoryDto(Guid ProductId, int Quantity, int ReservedQuantity, int AvailableQuantity);

public sealed class InventoryService(IInventoryRepository inventoryRepository)
{
    public async Task<Result<InventoryDto>> AdjustAsync(AdjustInventoryRequest request, CancellationToken cancellationToken)
    {
        var inventory = await inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        if (inventory is null) { inventory = new Domain.Entities.Inventory(request.ProductId, request.Quantity); await inventoryRepository.AddAsync(inventory, cancellationToken); }
        else inventory.Adjust(request.Quantity);
        await inventoryRepository.AddTransactionAsync(new InventoryTransaction(inventory.Id, request.Quantity, InventoryTransactionType.Adjustment, request.Reason), cancellationToken);
        await inventoryRepository.SaveChangesAsync(cancellationToken);
        return Result<InventoryDto>.Success(Map(inventory), "Inventory adjusted successfully.");
    }

    public async Task<Result<InventoryDto>> ReserveAsync(ReserveInventoryRequest request, CancellationToken cancellationToken)
    {
        var inventory = await inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        if (inventory is null) return Result<InventoryDto>.Failure("Inventory was not found.");
        inventory.Reserve(request.Quantity);
        await inventoryRepository.AddTransactionAsync(new InventoryTransaction(inventory.Id, request.Quantity, InventoryTransactionType.Reservation, request.Reason), cancellationToken);
        await inventoryRepository.SaveChangesAsync(cancellationToken);
        return Result<InventoryDto>.Success(Map(inventory), "Inventory reserved successfully.");
    }

    public async Task<Result<InventoryDto>> ReleaseAsync(ReserveInventoryRequest request, CancellationToken cancellationToken)
    {
        var inventory = await inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        if (inventory is null) return Result<InventoryDto>.Failure("Inventory was not found.");
        inventory.Release(request.Quantity);
        await inventoryRepository.AddTransactionAsync(new InventoryTransaction(inventory.Id, request.Quantity, InventoryTransactionType.Release, request.Reason), cancellationToken);
        await inventoryRepository.SaveChangesAsync(cancellationToken);
        return Result<InventoryDto>.Success(Map(inventory), "Inventory released successfully.");
    }

    private static InventoryDto Map(Domain.Entities.Inventory x) => new(x.ProductId, x.Quantity, x.ReservedQuantity, x.AvailableQuantity);
}
