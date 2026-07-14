using Microsoft.EntityFrameworkCore;
using sprint19_MinimalAPI.Data;
using sprint19_MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ========== Ендпоїнти для продуктів ==========

app.MapGet("/products", async (IProductRepository repo) =>
    {
        var products = await repo.GetAllAsync();
        return Results.Ok(products);
    })
    .WithName("GetAllProducts")
    .WithOpenApi();

app.MapGet("/products/{id:int}", async (int id, IProductRepository repo) =>
    {
        var product = await repo.GetByIdAsync(id);
        return product is not null ? Results.Ok(product) : Results.NotFound();
    })
    .WithName("GetProductById")
    .WithOpenApi();

app.MapPost("/products", async (Product product, IProductRepository repo) =>
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            return Results.BadRequest("Name is required");

        var created = await repo.AddAsync(product);
        return Results.Created($"/products/{created.Id}", created);
    })
    .WithName("CreateProduct")
    .WithOpenApi();

app.MapPut("/products/{id:int}", async (int id, Product product, IProductRepository repo) =>
    {
        if (id != product.Id)
            return Results.BadRequest("ID mismatch");
        if (string.IsNullOrWhiteSpace(product.Name))
            return Results.BadRequest("Name is required");

        var updated = await repo.UpdateAsync(id, product);
        return updated is not null ? Results.Ok(updated) : Results.NotFound();
    })
    .WithName("UpdateProduct")
    .WithOpenApi();

app.MapDelete("/products/{id:int}", async (int id, IProductRepository repo) =>
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    })
    .WithName("DeleteProduct")
    .WithOpenApi();

app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

app.Run();

