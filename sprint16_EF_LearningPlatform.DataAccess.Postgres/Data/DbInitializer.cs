using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Data;

public static class DbInitializer
{
    public static async Task SeedData(LearningDbContext context)
    {
        await context.Database.MigrateAsync();
        
        if (await context.Authors.AnyAsync())
        {
            return;
        }
        
        var author1 = new AuthorEntity 
        { 
            Id = Guid.NewGuid(), 
            UserName = "Steve Jobs" 
        };
        var author2 = new AuthorEntity 
        { 
            Id = Guid.NewGuid(), 
            UserName = "DevOps_Master" 
        };
        
        var course1 = new CourseEntity
        {
            Id = Guid.NewGuid(),
            AuthorId = author1.Id,
            Title = "Full-Stack Developer: .NET 10 & React",
            Description = "A comprehensive program for developing modern web applications.",
            Price = 2500
        };

        var course2 = new CourseEntity
        {
            Id = Guid.NewGuid(),
            AuthorId = author2.Id,
            Title = "DevOps & CI/CD Pipelines",
            Description = "Docker containerization and automation via GitHub Actions pipelines.",
            Price = 1800
        };
        
        var lessons = new List<LessonEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course1.Id,
                Title = "Introduction to Primary Constructors",
                Description = "Cleaning up code from redundant private fields.",
                LessonText = "Primary constructors allow you to declare parameters directly in the class header, making your code concise and clean..."
            },
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course1.Id,
                Title = "Query Optimization: ExecuteUpdate and ExecuteDelete",
                Description = "How to update data in the database without loading entities into memory.",
                LessonText = "The ExecuteUpdateAsync and ExecuteDeleteAsync methods send direct SQL queries to the database, bypassing the Change Tracker..."
            },
            new()
            {
                Id = Guid.NewGuid(),
                CourseId = course2.Id,
                Title = "Configuring Dockerfile for .NET Applications",
                Description = "Multi-stage builds to optimize Docker image size.",
                LessonText = "Using official SDK images for building and minimal ASP.NET runtime images for running helps significantly reduce the container size..."
            }
        };
        
        var student1 = new StudentEntity 
        { 
            Id = Guid.NewGuid(), 
            UserName = "Alex_Developer" 
        };
        var student2 = new StudentEntity 
        { 
            Id = Guid.NewGuid(), 
            UserName = "Maria_QA" 
        };
        
        student1.Courses.Add(course1);
        student1.Courses.Add(course2);
        student2.Courses.Add(course1);
        
        await context.Authors.AddRangeAsync(author1, author2);
        await context.Courses.AddRangeAsync(course1, course2);
        await context.Lessons.AddRangeAsync(lessons);
        await context.Students.AddRangeAsync(student1, student2);

        await context.SaveChangesAsync();
    }
}