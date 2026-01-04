using Entities;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Services.Tests;

public class ProductServiceTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        
        context.Products.AddRange(
            new Product { Id = 1, Name = "Espresso", Description = "Strong coffee", Price = 2.50m, StockQuantity = 100 },
            new Product { Id = 2, Name = "Cappuccino", Description = "Espresso with steamed milk foam", Price = 3.50m, StockQuantity = 100 },
            new Product { Id = 3, Name = "Latte", Description = "Espresso with steamed milk", Price = 4.00m, StockQuantity = 100 }
        );
        
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts_WithCorrectPrices()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new ProductService(context);

        // Act
        var products = await service.GetAllProductsAsync();

        // Assert
        Assert.Equal(3, products.Count);
        Assert.Contains(products, p => p.Name == "Espresso" && p.Price == 2.50m);
        Assert.Contains(products, p => p.Name == "Cappuccino" && p.Price == 3.50m);
        Assert.Contains(products, p => p.Name == "Latte" && p.Price == 4.00m);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsCorrectProduct_WhenProductExists()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new ProductService(context);

        // Act
        var product = await service.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
        Assert.Equal("Espresso", product.Name);
        Assert.Equal(2.50m, product.Price);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsCorrectProduct_ForProductId2()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new ProductService(context);

        // Act
        var product = await service.GetProductByIdAsync(2);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(2, product.Id);
        Assert.Equal("Cappuccino", product.Name);
        Assert.Equal(3.50m, product.Price);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new ProductService(context);

        // Act
        var product = await service.GetProductByIdAsync(999);

        // Assert
        Assert.Null(product);
    }

    [Fact]
    public async Task GetAllProductsAsync_DoesNotModifyDatabasePrices()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new ProductService(context);

        // Act
        var products = await service.GetAllProductsAsync();

        // Assert - Verify database prices are unchanged
        var dbProduct = await context.Products.FindAsync(1);
        Assert.Equal(2.50m, dbProduct!.Price);
    }
}
