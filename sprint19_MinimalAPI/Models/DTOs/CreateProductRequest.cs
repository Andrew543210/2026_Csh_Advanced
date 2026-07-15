namespace sprint19_MinimalAPI.Models.DTOs;

    public record CreateProductRequest(
        string Name,
        decimal Price,
        int CategoryId   
    );
