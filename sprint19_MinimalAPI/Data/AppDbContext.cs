using Microsoft.EntityFrameworkCore;
using sprint19_MinimalAPI.Models;

namespace sprint19_MinimalAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.CategoryEntity)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Categories with static values
        modelBuilder.Entity<Category>().HasData(
            new Category 
            { 
                Id = 1, 
                Name = "Electronics", 
                Description = "Gadgets, computers, and more",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category 
            { 
                Id = 2, 
                Name = "Clothing", 
                Description = "Fashion and apparel",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category 
            { 
                Id = 3, 
                Name = "Books", 
                Description = "Fiction and non-fiction",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Seed Products with static values
        modelBuilder.Entity<Product>().HasData(
            new Product 
            { 
                Id = 1, 
                Name = "Laptop", 
                Price = 999.99m, 
                CategoryId = 1,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new Product 
            { 
                Id = 2, 
                Name = "T-Shirt", 
                Price = 19.99m, 
                CategoryId = 2,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new Product 
            { 
                Id = 3, 
                Name = "Book", 
                Price = 12.50m, 
                CategoryId = 3,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        );
    }
}