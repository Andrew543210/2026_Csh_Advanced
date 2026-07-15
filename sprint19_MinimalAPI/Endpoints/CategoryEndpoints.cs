using FluentValidation;
using sprint19_MinimalAPI.Common;
using sprint19_MinimalAPI.Models;
using sprint19_MinimalAPI.Models.DTOs;
using sprint19_MinimalAPI.Services;

namespace sprint19_MinimalAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithTags("Categories")
            .WithOpenApi();

        group.MapGet("/", GetAllCategories).WithName("GetAllCategories").WithOpenApi();
        group.MapGet("/{id:int}", GetCategoryById).WithName("GetCategoryById").WithOpenApi();
        group.MapPost("/", CreateCategory).WithName("CreateCategory").WithOpenApi();
        group.MapPut("/{id:int}", UpdateCategory).WithName("UpdateCategory").WithOpenApi();
        group.MapDelete("/{id:int}", DeleteCategory).WithName("DeleteCategory").WithOpenApi();
        group.MapGet("/{id:int}/products", GetProductsByCategory).WithName("GetProductsByCategory").WithOpenApi();
    }

    private static async Task<IResult> GetAllCategories(ICategoryService service)
    {
        var categories = await service.GetAllAsync();
        var response = categories.Select(c => new CategoryResponse(c.Id, c.Name, c.Description, c.CreatedAt));
        return Results.Ok(ApiResponse.Success(response));
    }

    private static async Task<IResult> GetCategoryById(int id, ICategoryService service)
    {
        var category = await service.GetByIdAsync(id);
        if (category == null)
            return Results.NotFound(ApiResponse.Error("Category not found"));

        var response = new CategoryResponse(category.Id, category.Name, category.Description, category.CreatedAt);
        return Results.Ok(ApiResponse.Success(response));
    }

    private static async Task<IResult> CreateCategory(
        CreateCategoryRequest request,
        ICategoryService service,
        IValidator<CreateCategoryRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Results.BadRequest(ApiResponse.Error(errors));
        }

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        var created = await service.CreateAsync(category);
        var response = new CategoryResponse(created.Id, created.Name, created.Description, created.CreatedAt);
        return Results.Created($"/api/categories/{created.Id}", ApiResponse.Success(response));
    }

    private static async Task<IResult> UpdateCategory(
        int id,
        UpdateCategoryRequest request,
        ICategoryService service,
        IValidator<UpdateCategoryRequest> validator)
    {
        if (id != request.Id)
            return Results.BadRequest(ApiResponse.Error("ID mismatch"));

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Results.BadRequest(ApiResponse.Error(errors));
        }

        var category = new Category
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        };

        var updated = await service.UpdateAsync(id, category);
        if (updated == null)
            return Results.NotFound(ApiResponse.Error("Category not found"));

        var response = new CategoryResponse(updated.Id, updated.Name, updated.Description, updated.CreatedAt);
        return Results.Ok(ApiResponse.Success(response));
    }

    private static async Task<IResult> DeleteCategory(int id, ICategoryService service)
    {
        var deleted = await service.DeleteAsync(id);
        if (!deleted)
            return Results.NotFound(ApiResponse.Error("Category not found"));

        return Results.NoContent();
    }

    private static async Task<IResult> GetProductsByCategory(
        int id,
        IProductService productService,
        ICategoryService categoryService)
    {
        var category = await categoryService.GetByIdAsync(id);
        if (category == null)
            return Results.NotFound(ApiResponse.Error("Category not found"));

        var products = await productService.GetAllAsync();
        var filtered = products.Where(p => p.CategoryId == id).ToList();
        var response = filtered.Select(p => new ProductResponse(
            p.Id, p.Name, p.Price, p.CategoryId, p.CategoryEntity?.Name, p.CreatedAt));
        return Results.Ok(ApiResponse.Success(response));
    }
}