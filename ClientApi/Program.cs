using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Requests;
using Services;
using System.Text.Json.Serialization;

namespace ClientApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("CafeDb"));

        builder.Services.AddScoped<ProductService>();
        builder.Services.AddScoped<OrderService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        }

        app.UseCors("AllowAll");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cafe API v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapGet("/products", async (ProductService productService) =>
        {
            var products = await productService.GetAllProductsAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .WithOpenApi();

        app.MapGet("/products/{id}", async (int id, ProductService productService) =>
        {
            var product = await productService.GetProductByIdAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithOpenApi();

        app.MapPost("/orders", async (CreateOrderRequest request, OrderService orderService) =>
        {
            try
            {
                var items = request.Items.Select(i => new OrderItemRequest
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList();

                var order = await orderService.CreateOrderAsync(request.CustomerName, items);
                return Results.Created($"/orders/{order.Id}", order);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("CreateOrder")
        .WithOpenApi();

        app.Run();
    }
}
