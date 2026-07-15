using sprint19_MinimalAPI.Models;


namespace sprint19_MinimalAPI.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(int id, Category category);
    Task<bool> DeleteAsync(int id);
}