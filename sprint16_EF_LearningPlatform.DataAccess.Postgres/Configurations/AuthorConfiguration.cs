using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
        builder.HasKey(a => a.Id);
        
    }
}