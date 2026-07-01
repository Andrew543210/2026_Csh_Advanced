using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;

public class LessonsRepository
{
    private readonly LearningDbContext _context;

    public LessonsRepository(LearningDbContext context)
    {
        _context = context;
    }

    public async Task Add(Guid courseId, LessonEntity lesson)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId) ??
                     throw new Exception("Course not found");
        course.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task Add2(Guid courseId, string title, string description, string lessontext)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId) ??
                     throw new Exception("Course not found");
        course.Lessons.Add(new LessonEntity
        {
            Title = title,
            Description = description,
            LessonText = lessontext
        });
        await _context.SaveChangesAsync();
    }
}