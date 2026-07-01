using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<StudentEntity>
{
    public void Configure(EntityTypeBuilder<StudentEntity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasMany(s => s.Courses)
            .WithMany(c => c.Students);
    }
}