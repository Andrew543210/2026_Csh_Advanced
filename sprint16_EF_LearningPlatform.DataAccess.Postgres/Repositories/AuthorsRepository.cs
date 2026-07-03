using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;

public class AuthorsRepository(LearningDbContext context)
{
    public async Task Add(Guid id, string userName)
    {
        var author = new AuthorEntity
        {
            Id = id,
            UserName = userName
        };
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
    }

    public async Task<List<AuthorEntity>> Get()
    {
        return await context.Authors.AsNoTracking().ToListAsync();
    }

    public async Task Delete(Guid id)
    {
        await context.Authors.Where(a => a.Id == id).ExecuteDeleteAsync();
    }
}