using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Product>(e =>
        {
            e.Property(p => p.ProductCode).IsRequired();
            e.Property(p => p.ProductName).IsRequired();
            e.Property(p => p.Price).HasPrecision(18, 2);
            e.HasIndex(p => p.ProductCode).IsUnique(); // enforce unique ProductCode
        });
    }
}
