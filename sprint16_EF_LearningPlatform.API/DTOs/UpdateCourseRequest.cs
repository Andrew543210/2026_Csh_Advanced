namespace sprint16_EF_LearningPlatform.API.DTOs;

public record UpdateCourseRequest(Guid authorId, string title, string description, decimal price);