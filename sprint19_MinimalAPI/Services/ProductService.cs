using Microsoft.EntityFrameworkCore;
using sprint19_MinimalAPI.Data;
using sprint19_MinimalAPI.Models;
using sprint19_MinimalAPI.Models;

namespace sprint19_MinimalAPI.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.CategoryEntity)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.CategoryEntity)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null || !existing.IsActive) return null;

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.CategoryId = product.CategoryId; 

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        product.IsActive = false; 
        await _context.SaveChangesAsync();
        return true;
    }
}