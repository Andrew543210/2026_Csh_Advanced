using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;

public class CoursesRepository
{
    private readonly LearningDbContext _context;
    
    public CoursesRepository(LearningDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<CourseEntity>> Get()
    {
        return await _context.Courses
                        .AsNoTracking()
                        .OrderBy(c => c.Title)
                        .ToListAsync();
    }

    public async Task<List<CourseEntity>> GetWithLessons()
    {
        return await _context.Courses
                        .Include(c => c.Lessons)
                        .AsNoTracking()
                        .ToListAsync();
    }
    
    public async Task<CourseEntity?> GetById(Guid id)
    {
        return await _context.Courses
                                .AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<CourseEntity>> GetByFilter(string title, decimal price)
    {
        var query = _context.Courses.AsNoTracking();
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(c => c.Title.Contains(title));
        }
        if (price > 0)
        {
            query = query.Where(c => c.Price >= price);
        }
        return await query.ToListAsync();
    }

    public async Task<List<CourseEntity>> GetByPage(int page, int pagesize)
    {
        return await _context.Courses
                        .AsNoTracking()
                        .OrderBy(c => c.Title)
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToListAsync();
    }

    public async Task Add(Guid id, Guid authorId, string title, string description, decimal price)
    {
        var course = new CourseEntity
        {
            Id = id,
            AuthorId = authorId,
            Title = title,
            Description = description,
            Price = price
        };
        await _context.AddAsync(course);
        await _context.SaveChangesAsync();
    }
    
    public async Task Update(Guid id, Guid authorId, string title, string description, decimal price)
    {
        await _context.Courses
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(c => c
                .SetProperty(c => c.AuthorId, authorId)
                .SetProperty(c => c.Title, title)
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.Price, price));
    }
    
    // public async Task Update(Guid id, Guid authorId, string title, string description, decimal price)
    // {
    //     var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
    //     if (course != null)
    //     {
    //         course.AuthorId = authorId;
    //         course.Title = title;
    //         course.Description = description;
    //         course.Price = price;
    //     }
    //     await _context.SaveChangesAsync();
    // }
    
    
    public async Task Delete(Guid id)
    {
       await _context.Courses
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();
    }
}