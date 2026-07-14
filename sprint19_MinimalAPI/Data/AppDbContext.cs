using Microsoft.EntityFrameworkCore;
using sprint19_MinimalAPI.Models;

namespace sprint19_MinimalAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics" },
            new Product { Id = 2, Name = "T-Shirt", Price = 19.99m, Category = "Clothing" },
            new Product { Id = 3, Name = "Book", Price = 12.50m, Category = "Books" }
        );
    }
}