namespace sprint19_MinimalAPI.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
   
    public int CategoryId { get; set; }                   
    public Category? CategoryEntity { get; set; }        
   
}