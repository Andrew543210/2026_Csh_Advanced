using FluentValidation;
using Microsoft.EntityFrameworkCore;
using sprint19_MinimalAPI.Data;
using sprint19_MinimalAPI.Endpoints;
using sprint19_MinimalAPI.Middleware;
using sprint19_MinimalAPI.Models.DTOs;
using sprint19_MinimalAPI.Services;
using sprint19_MinimalAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Реєстрація валідаторів
builder.Services.AddScoped<IValidator<CreateProductRequest>, CreateProductValidator>();
builder.Services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductValidator>();
builder.Services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryValidator>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ========== Endpoints ==========
app.MapProductEndpoints();
app.MapCategoryEndpoints();

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

app.Run();