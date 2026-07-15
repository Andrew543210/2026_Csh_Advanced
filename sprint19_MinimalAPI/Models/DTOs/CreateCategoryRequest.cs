namespace sprint19_MinimalAPI.Models.DTOs;

public record CreateCategoryRequest(
    string Name,
    string? Description
);