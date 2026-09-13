using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Interfaces;

public interface IReviewRepository
{
    Task<Review?> GetByUserAndProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
