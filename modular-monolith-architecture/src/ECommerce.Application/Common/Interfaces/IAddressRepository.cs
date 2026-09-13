using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Interfaces;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(Guid userId, Guid addressId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Address>> ListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Address address, CancellationToken cancellationToken = default);
    void Remove(Address address);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
