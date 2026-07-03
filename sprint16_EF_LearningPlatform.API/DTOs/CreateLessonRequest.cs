namespace sprint16_EF_LearningPlatform.API.DTOs;

public record CreateLessonRequest(Guid id,Guid courseId, string title, string description, string lessonText);