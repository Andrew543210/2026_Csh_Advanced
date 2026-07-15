namespace sprint19_MinimalAPI.Models.DTOs;

public record UpdateProductRequest(
    int Id,
    string Name,
    decimal Price,
    int CategoryId
);