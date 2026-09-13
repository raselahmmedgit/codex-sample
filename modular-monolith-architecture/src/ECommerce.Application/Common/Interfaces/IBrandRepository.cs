using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Interfaces;

public interface IBrandRepository
{
    Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Brand>> ListAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Brand brand, CancellationToken cancellationToken = default);
    void Remove(Brand brand);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
