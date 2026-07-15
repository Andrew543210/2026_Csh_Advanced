namespace sprint19_MinimalAPI.Models.DTOs;

public record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    int CategoryId,
    string? CategoryName,
    DateTime CreatedAt
);