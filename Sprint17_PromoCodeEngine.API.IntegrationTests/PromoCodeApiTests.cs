using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Sprint17_PromoCodeEngine.API.IntegrationTests;

[TestFixture]
public class PromoCodeApiTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task ApplyPromoCode_ShouldReturnOk_WhenPromoCodeIsValid()
    {
        var requestData = new { Code = "SUMMER10", Amount = 100m };

        var response = await _client.PostAsJsonAsync("/api/promocodes/apply", requestData);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, decimal>>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result["finalAmount"], Is.EqualTo(90m));
    }

    [Test]
    public async Task ApplyPromoCode_ShouldReturnBadRequest_WhenCodeIsEmpty()
    {
        var requestData = new { Code = "", Amount = 100m };

        var response = await _client.PostAsJsonAsync("/api/promocodes/apply", requestData);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task ApplyPromoCode_ShouldReturnBadRequest_WhenAmountIsZeroOrNegative()
    {
        var requestData = new { Code = "SUMMER10", Amount = -50m };

        var response = await _client.PostAsJsonAsync("/api/promocodes/apply", requestData);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task ApplyPromoCode_ShouldReturnBadRequest_WhenPromoCodeNotFound()
    {
        var requestData = new { Code = "UNKNOWN_CODE", Amount = 100m };

        var response = await _client.PostAsJsonAsync("/api/promocodes/apply", requestData);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}