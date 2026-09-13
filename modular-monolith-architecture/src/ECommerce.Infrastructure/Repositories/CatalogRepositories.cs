using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class CategoryRepository(ApplicationDbContext dbContext) : ICategoryRepository
{
    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    public async Task<IReadOnlyCollection<Category>> ListAsync(CancellationToken cancellationToken = default) => await dbContext.Categories.AsNoTracking().OrderBy(x => x.SortOrder).ThenBy(x => x.Name).ToArrayAsync(cancellationToken);
    public Task AddAsync(Category category, CancellationToken cancellationToken = default) => dbContext.Categories.AddAsync(category, cancellationToken).AsTask();
    public void Remove(Category category) => dbContext.Categories.Remove(category);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}

public sealed class BrandRepository(ApplicationDbContext dbContext) : IBrandRepository
{
    public Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => dbContext.Brands.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    public async Task<IReadOnlyCollection<Brand>> ListAsync(CancellationToken cancellationToken = default) => await dbContext.Brands.AsNoTracking().OrderBy(x => x.Name).ToArrayAsync(cancellationToken);
    public Task AddAsync(Brand brand, CancellationToken cancellationToken = default) => dbContext.Brands.AddAsync(brand, cancellationToken).AsTask();
    public void Remove(Brand brand) => dbContext.Brands.Remove(brand);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => dbContext.SaveChangesAsync(cancellationToken);
}
