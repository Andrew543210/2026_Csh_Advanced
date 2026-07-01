namespace sprint16_EF_LearningPlatform.API.DTOs;

public record CreateCourseRequest(Guid id, Guid authorId, string title, string description, decimal price);