namespace sprint19_MinimalAPI.Models.DTOs;

public record CategoryResponse(
    int Id,
    string Name,
    string? Description,
    DateTime CreatedAt
);