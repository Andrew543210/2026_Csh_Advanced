using Microsoft.EntityFrameworkCore;
using sprint16_EF_LearningPlatform.DataAccess.Postgres;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;

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

app.Run();