using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;

public class LessonsRepository
{
    private readonly LearningDbContext _context;

    public LearningDbContext Context => _context;

    public LessonsRepository(LearningDbContext context)
    {
        _context = context;
    }
    
    public async Task Add(Guid id, Guid courseId, string title, string description, string lessontext)
    {
        var lesson = new LessonEntity
        {
            Id = id,
            CourseId = courseId,
            Title = title,
            Description = description,
            LessonText = lessontext
        };

        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task<LessonEntity?> GetById(Guid id)
    {
        return await _context.Lessons.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<List<LessonEntity>> GetByCourseId(Guid courseId)
    {
        return await _context.Lessons.Where(l => l.CourseId == courseId).AsNoTracking().ToListAsync();
    }

    public async Task Delete(Guid id)
    {
        await _context.Lessons.Where(l => l.Id == id).ExecuteDeleteAsync();
    }

    public async Task Update(Guid id, string title, string description, string lessontext)
    {
        await _context.Lessons
            .Where(l => l.Id == id)
            .ExecuteUpdateAsync(l => l
                .SetProperty(l => l.Title, title)
                .SetProperty(l => l.Description, description)
                .SetProperty(l => l.LessonText, lessontext));
    }
}