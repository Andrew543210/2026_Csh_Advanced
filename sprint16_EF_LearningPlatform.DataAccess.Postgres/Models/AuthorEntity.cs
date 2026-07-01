namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

public class AuthorEntity
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    
    public CourseEntity? Course { get; set; }
    
}