namespace sprint16_EF_LearningPlatform.DataAccess.Postgres.Models;

public class StudentEntity
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    
    public List<CourseEntity> Courses { get; set; } = [];
}