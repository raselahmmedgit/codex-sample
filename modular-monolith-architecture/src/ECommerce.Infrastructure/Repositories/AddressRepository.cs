using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class AddressRepository(ApplicationDbContext dbContext) : IAddressRepository
{
    public Task<Address?> GetByIdAsync(Guid userId, Guid addressId, CancellationToken cancellationToken = default) => dbContext.Addresses.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == addressId, cancellationToken);
    public async Task<IReadOnlyCollection<Address>> ListAsync(Guid userId, CancellationToken cancellationToken = default) => await dbContext.Addresses.AsNoTracking().Where(x => x.UserId == userId).OrderByDescending(x => x.IsDefault).ThenBy(x => x.CreatedAtUtc).ToArrayAsync(cancellationToken);
    public Task AddAsync(Address address, CancellationToken cancellationToken = default) => dbContext.Addresses.AddAsync(address, cancellationToken).AsTask();
    public void Remove(Address address) => dbContext.Addresses.Remove(address);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}
