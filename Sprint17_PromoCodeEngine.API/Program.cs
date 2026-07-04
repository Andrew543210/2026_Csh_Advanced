using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sprint17_PromoCodeEngine.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

var repositoryMock = Substitute.For<IPromoCodeRepository>();
var fakePromo = new PromoCode { Percentage = 10, IsActive = true, ExpirationDate = DateTime.UtcNow.AddDays(1) };
repositoryMock.GetByCodeAsync("SUMMER10").Returns(fakePromo);

builder.Services.AddSingleton(repositoryMock);
builder.Services.AddScoped<PromoCodeService>();

var app = builder.Build();

app.MapPost("/api/promocodes/apply", async ([FromBody] ApplyPromoCodeRequest request, PromoCodeService service) =>
{
    try
    {
        var finalAmount = await service.ApplyDiscountAsync(request.Code, request.Amount);
        return Results.Ok(new { OriginalAmount = request.Amount, FinalAmount = finalAmount });
    }
    catch (ArgumentOutOfRangeException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { Error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.UnprocessableEntity(new { Error = ex.Message });
    }
});

app.Run();

public record ApplyPromoCodeRequest(string Code, decimal Amount);

public partial class Program { }