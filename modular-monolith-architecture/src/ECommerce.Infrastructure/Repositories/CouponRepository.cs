using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class CouponRepository(ApplicationDbContext dbContext) : ICouponRepository
{
    public Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default) => dbContext.Coupons.FirstOrDefaultAsync(x => x.Code == code.Trim().ToUpper(), cancellationToken);
    public Task AddAsync(Coupon coupon, CancellationToken cancellationToken = default) => dbContext.Coupons.AddAsync(coupon, cancellationToken).AsTask();
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}
