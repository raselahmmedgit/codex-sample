using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Coupons;

public sealed record ValidateCouponRequest(string Code, decimal OrderAmount);
public sealed record CreateCouponRequest(string Code, CouponType Type, decimal Value, DateTime StartsAtUtc, DateTime ExpiresAtUtc);
public sealed record CouponDiscountDto(string Code, decimal DiscountAmount);

public sealed class CouponService(ICouponRepository couponRepository)
{
    public async Task<Result<CouponDiscountDto>> ValidateAsync(ValidateCouponRequest request, CancellationToken cancellationToken)
    {
        if (request.OrderAmount < 0) return Result<CouponDiscountDto>.Failure("Order amount cannot be negative.");
        var coupon = await couponRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (coupon is null || !coupon.IsValid(DateTime.UtcNow, request.OrderAmount)) return Result<CouponDiscountDto>.Failure("Coupon is invalid or expired.");
        return Result<CouponDiscountDto>.Success(new(coupon.Code, coupon.CalculateDiscount(request.OrderAmount)), "Coupon is valid.");
    }

    public async Task<Result<bool>> CreateAsync(CreateCouponRequest request, CancellationToken cancellationToken)
    {
        var coupon = new Coupon(request.Code, request.Type, request.Value, request.StartsAtUtc, request.ExpiresAtUtc);
        await couponRepository.AddAsync(coupon, cancellationToken); await couponRepository.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true, "Coupon created successfully.");
    }
}
