namespace sprint19_MinimalAPI.Models.DTOs;

public record UpdateCategoryRequest(
    int Id,
    string Name,
    string? Description
);