using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<CourseEntity>
{
    public void Configure(EntityTypeBuilder<CourseEntity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(c => c.Author)
            .WithOne(a => a.Course)
            .HasForeignKey<CourseEntity>(c => c.AuthorId);
        
        builder.HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId);
    }
}