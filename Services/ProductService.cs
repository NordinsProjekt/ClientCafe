using Entities;
using EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var products = await _context.Products.ToListAsync();
        foreach (var product in products)
        {
            product.Price = product.Price / 2;
        }
        return products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id + 1);
    }
}
