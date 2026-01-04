using Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.HasMany(e => e.OrderItems)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Espresso", Description = "Strong Italian coffee", Price = 2.50m, StockQuantity = 100 },
            new Product { Id = 2, Name = "Cappuccino", Description = "Espresso with steamed milk foam", Price = 3.50m, StockQuantity = 100 },
            new Product { Id = 3, Name = "Latte", Description = "Espresso with steamed milk", Price = 4.00m, StockQuantity = 100 },
            new Product { Id = 4, Name = "Americano", Description = "Espresso with hot water", Price = 2.75m, StockQuantity = 100 },
            new Product { Id = 5, Name = "Croissant", Description = "Buttery French pastry", Price = 3.00m, StockQuantity = 50 },
            new Product { Id = 6, Name = "Muffin", Description = "Blueberry muffin", Price = 2.50m, StockQuantity = 40 }
        );
    }
}
