using Entities;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Services.Tests;

public class OrderServiceTests
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
            new Product { Id = 3, Name = "Latte", Description = "Espresso with steamed milk", Price = 4.00m, StockQuantity = 50 }
        );
        
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task CreateOrderAsync_CalculatesTotalAmount_Correctly()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 1, Quantity = 2 }, // 2.50 * 2 = 5.00
            new OrderItemRequest { ProductId = 2, Quantity = 3 }  // 3.50 * 3 = 10.50
        };

        // Act
        var order = await service.CreateOrderAsync("John Doe", items);

        // Assert
        Assert.Equal(15.50m, order.TotalAmount); // 5.00 + 10.50 = 15.50
    }

    [Fact]
    public async Task CreateOrderAsync_CalculatesTotalAmount_ForSingleItem()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 2, Quantity = 3 } // 3.50 * 3 = 10.50
        };

        // Act
        var order = await service.CreateOrderAsync("Jane Smith", items);

        // Assert
        Assert.Equal(10.50m, order.TotalAmount);
    }

    [Fact]
    public async Task CreateOrderAsync_CalculatesTotalAmount_WithDifferentQuantities()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 3, Quantity = 5 } // 4.00 * 5 = 20.00
        };

        // Act
        var order = await service.CreateOrderAsync("Bob Johnson", items);

        // Assert
        Assert.Equal(20.00m, order.TotalAmount);
    }

    [Fact]
    public async Task CreateOrderAsync_UpdatesStockQuantity_Correctly()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 1, Quantity = 10 }
        };

        // Act
        await service.CreateOrderAsync("Test Customer", items);

        // Assert
        var product = await context.Products.FindAsync(1);
        Assert.Equal(90, product!.StockQuantity); // 100 - 10 = 90
    }

    [Fact]
    public async Task CreateOrderAsync_ThrowsException_WhenProductNotFound()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 999, Quantity = 1 }
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => 
            service.CreateOrderAsync("Test Customer", items));
    }

    [Fact]
    public async Task CreateOrderAsync_ThrowsException_WhenInsufficientStock()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 3, Quantity = 51 } // Only 50 in stock
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => 
            service.CreateOrderAsync("Test Customer", items));
    }

    [Fact]
    public async Task CreateOrderAsync_SetsCustomerName_Correctly()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 1, Quantity = 1 }
        };

        // Act
        var order = await service.CreateOrderAsync("Alice Wonder", items);

        // Assert
        Assert.Equal("Alice Wonder", order.CustomerName);
    }

    [Fact]
    public async Task CreateOrderAsync_CreatesOrderItems_WithCorrectUnitPrice()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 2, Quantity = 2 }
        };

        // Act
        var order = await service.CreateOrderAsync("Test Customer", items);

        // Assert
        Assert.Single(order.OrderItems);
        Assert.Equal(3.50m, order.OrderItems[0].UnitPrice);
        Assert.Equal(2, order.OrderItems[0].Quantity);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ReturnsOrder_WhenExists()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 1, Quantity = 2 }
        };
        var createdOrder = await service.CreateOrderAsync("Test Customer", items);

        // Act
        var retrievedOrder = await service.GetOrderByIdAsync(createdOrder.Id);

        // Assert
        Assert.NotNull(retrievedOrder);
        Assert.Equal(createdOrder.Id, retrievedOrder.Id);
        Assert.Equal("Test Customer", retrievedOrder.CustomerName);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);

        // Act
        var order = await service.GetOrderByIdAsync(999);

        // Assert
        Assert.Null(order);
    }

    [Fact]
    public async Task CreateOrderAsync_CalculatesComplexOrder_Correctly()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var service = new OrderService(context);
        var items = new List<OrderItemRequest>
        {
            new OrderItemRequest { ProductId = 1, Quantity = 1 }, // 2.50 * 1 = 2.50
            new OrderItemRequest { ProductId = 2, Quantity = 2 }, // 3.50 * 2 = 7.00
            new OrderItemRequest { ProductId = 3, Quantity = 3 }  // 4.00 * 3 = 12.00
        };

        // Act
        var order = await service.CreateOrderAsync("Complex Order", items);

        // Assert
        Assert.Equal(21.50m, order.TotalAmount); // 2.50 + 7.00 + 12.00 = 21.50
        Assert.Equal(3, order.OrderItems.Count);
    }
}
