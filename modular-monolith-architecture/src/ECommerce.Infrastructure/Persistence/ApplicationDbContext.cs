using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options);
