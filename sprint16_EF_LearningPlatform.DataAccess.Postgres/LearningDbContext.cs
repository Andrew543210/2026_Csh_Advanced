using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Configurations;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres;

public class LearningDbContext(DbContextOptions<LearningDbContext> options) : DbContext(options)
{
    
    public DbSet<CourseEntity> Courses { get; set; }
    
    public DbSet<LessonEntity> Lessons { get; set; }
    
    public DbSet<AuthorEntity> Authors { get; set; }
    
    public DbSet<StudentEntity> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new LessonConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}