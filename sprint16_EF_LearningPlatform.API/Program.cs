using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;
using sprint16_EF_LearningPlatform.DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<LearningDbContext>(options =>
    {
options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
    }
);

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<LessonsRepository>();
builder.Services.AddScoped<AuthorsRepository>();
builder.Services.AddScoped<StudentsRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Learning Platform API v1");
        options.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LearningDbContext>();
        await DbInitializer.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

app.Run();