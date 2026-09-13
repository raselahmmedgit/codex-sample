using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Reviews;

public sealed record CreateReviewRequest(Guid ProductId, int Rating, string Comment);
public sealed record ReviewDto(Guid Id, Guid ProductId, Guid UserId, int Rating, string Comment, ReviewModerationStatus ModerationStatus);

public sealed class ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository)
{
    public async Task<Result<ReviewDto>> CreateAsync(Guid userId, CreateReviewRequest request, CancellationToken cancellationToken)
    {
        if (!await orderRepository.HasDeliveredPurchaseAsync(userId, request.ProductId, cancellationToken)) return Result<ReviewDto>.Failure("Only verified purchasers can review this product.");
        if (await reviewRepository.GetByUserAndProductAsync(userId, request.ProductId, cancellationToken) is not null) return Result<ReviewDto>.Failure("You have already reviewed this product.");
        var review = new Review(request.ProductId, userId, request.Rating, request.Comment);
        await reviewRepository.AddAsync(review, cancellationToken); await reviewRepository.SaveChangesAsync(cancellationToken);
        return Result<ReviewDto>.Success(Map(review), "Review submitted for moderation.");
    }

    private static ReviewDto Map(Review x) => new(x.Id, x.ProductId, x.UserId, x.Rating, x.Comment, x.ModerationStatus);
}
