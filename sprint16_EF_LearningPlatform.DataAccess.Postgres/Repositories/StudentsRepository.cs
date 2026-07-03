using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;

public class StudentsRepository(LearningDbContext context)
{
    public async Task Add(Guid id, string userName)
    {
        var student = new StudentEntity
        {
            Id = id,
            UserName = userName
        };
        await context.Students.AddAsync(student);
        await context.SaveChangesAsync();
    }

    public async Task<List<StudentEntity>> Get()
    {
        return await context.Students.AsNoTracking().ToListAsync();
    }
    
    public async Task Enroll(Guid studentId, Guid courseId)
    {
        var student = await context.Students
            .Include(s => s.Courses)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        var course = await context.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (student != null && course != null)
        {
            if (!student.Courses.Any(c => c.Id == courseId))
            {
                student.Courses.Add(course);
                await context.SaveChangesAsync();
            }
        }
    }
    
    public async Task<List<StudentEntity>> GetWithCourses()
    {
        return await context.Students
            .Include(s => s.Courses)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task Delete(Guid id)
    {
        await context.Students.Where(s => s.Id == id).ExecuteDeleteAsync();
    }
}