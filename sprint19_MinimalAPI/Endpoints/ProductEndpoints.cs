using FluentValidation;
using sprint19_MinimalAPI.Common;
using sprint19_MinimalAPI.Models;
using sprint19_MinimalAPI.Models.DTOs;
using sprint19_MinimalAPI.Models;
using sprint19_MinimalAPI.Services;

namespace sprint19_MinimalAPI.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        group.MapGet("/", GetAllProducts).WithName("GetAllProducts").WithOpenApi();
        group.MapGet("/{id:int}", GetProductById).WithName("GetProductById").WithOpenApi();
        group.MapPost("/", CreateProduct).WithName("CreateProduct").WithOpenApi();
        group.MapPut("/{id:int}", UpdateProduct).WithName("UpdateProduct").WithOpenApi();
        group.MapDelete("/{id:int}", DeleteProduct).WithName("DeleteProduct").WithOpenApi();
    }

    private static async Task<IResult> GetAllProducts(IProductService service)
    {
        var products = await service.GetAllAsync();
        var response = products.Select(p => new ProductResponse(
            p.Id, p.Name, p.Price, p.CategoryId, p.CategoryEntity?.Name, p.CreatedAt));
        return Results.Ok(ApiResponse.Success(response));
    }

    private static async Task<IResult> GetProductById(int id, IProductService service)
    {
        var product = await service.GetByIdAsync(id);
        if (product == null)
            return Results.NotFound(ApiResponse.Error("Product not found"));

        var response = new ProductResponse(
            product.Id, product.Name, product.Price, product.CategoryId, 
            product.CategoryEntity?.Name, product.CreatedAt);
        return Results.Ok(ApiResponse.Success(response));
    }

    private static async Task<IResult> CreateProduct(
        CreateProductRequest request,
        IProductService service,
        IValidator<CreateProductRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Results.BadRequest(ApiResponse.Error(errors));
        }

        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        var created = await service.CreateAsync(product);
        var response = new ProductResponse(
            created.Id, created.Name, created.Price, created.CategoryId,
            (await service.GetByIdAsync(created.Id))?.CategoryEntity?.Name,
            created.CreatedAt);

        return Results.Created($"/api/products/{created.Id}", ApiResponse.Success(response));
    }

    private static async Task<IResult> UpdateProduct(
        int id,
        UpdateProductRequest request,
        IProductService service,
        IValidator<UpdateProductRequest> validator)
    {
        if (id != request.Id)
            return Results.BadRequest(ApiResponse.Error("ID mismatch"));

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Results.BadRequest(ApiResponse.Error(errors));
        }

        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        var updated = await service.UpdateAsync(id, product);
        if (updated == null)
            return Results.NotFound(ApiResponse.Error("Product not found"));

        var response = new ProductResponse(
            updated.Id, updated.Name, updated.Price, updated.CategoryId,
            updated.CategoryEntity?.Name, updated.CreatedAt);
        return Results.Ok(ApiResponse.Success(response));
    }

    private static async Task<IResult> DeleteProduct(int id, IProductService service)
    {
        var deleted = await service.DeleteAsync(id);
        if (!deleted)
            return Results.NotFound(ApiResponse.Error("Product not found"));

        return Results.NoContent();
    }
}