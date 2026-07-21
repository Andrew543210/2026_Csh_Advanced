using FluentValidation;
using Sprint20_FluentValidation.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.Configure<ValidationSettings>(
    builder.Configuration.GetSection("ValidationSettings"));
builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/register", async (UserRegistrationDto registration, IValidator<UserRegistrationDto> validator) =>
    {
        var validationResult = await validator.ValidateAsync(registration);

        if (!validationResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = "One or more validation errors occurred.",
                Instance = "/api/register"
            };

            return Results.Problem(problemDetails);
        }

       
        return Results.Ok(new { message = "User registered successfully", email = registration.Email });
    })
    .WithName("RegisterUser")
    .WithOpenApi();

app.Run();

