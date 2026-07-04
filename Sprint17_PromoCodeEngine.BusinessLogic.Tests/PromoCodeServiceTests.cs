using NSubstitute;

namespace Sprint17_PromoCodeEngine.BusinessLogic.Tests;

[TestFixture]
public class PromoCodeServiceTests
{
    private IPromoCodeRepository _repositoryMock;
    private PromoCodeService _service;
    
    [SetUp]
    public void Setup()
    {
        _repositoryMock = Substitute.For<IPromoCodeRepository>();
        _service = new PromoCodeService(_repositoryMock);
    }

    [Test]
    public void ApplyDiscountAsync_ShouldThrowArgumentException_WhenPromoCodeNotFound()
    {
        Assert.That(async () => await _service.ApplyDiscountAsync("INVALID_CODE", 100m), Throws.ArgumentException);
    }

    [Test]
    public async Task ApplyDiscountAsync_ShouldReturnDiscountedPrice_WhenPromoCodeIsValid()
    {
        var promocode = new PromoCode { Percentage = 10, IsActive = true, ExpirationDate = DateTime.UtcNow.AddDays(1) };
        _repositoryMock.GetByCodeAsync("SUMMER_10").Returns(promocode);

        var result = await _service.ApplyDiscountAsync("SUMMER_10", 100m);
        
        Assert.That(result, Is.EqualTo(90m));
    }

    [Test]
    public void ApplyDiscountAsync_ShouldThrowInvalidOperationException_WhenPromoCodeIsInactive()
    {
        var promocode = new PromoCode { Percentage = 10, IsActive = false };
        _repositoryMock.GetByCodeAsync("SUMMER_10").Returns(promocode);
        
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.ApplyDiscountAsync("SUMMER_10", 100m));
        Assert.That(ex.Message, Is.EqualTo("Promo code is inactive."));
    }

    [Test]
    public void ApplyDiscountAsync_ShouldThrowInvalidOperationException_WhenPromoCodeHasExpired()
    {
        var promocode = new PromoCode{ Percentage = 10, IsActive = true, ExpirationDate = DateTime.UtcNow.AddDays(-1) };
        _repositoryMock.GetByCodeAsync("SUMMER_10").Returns(promocode);
        
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.ApplyDiscountAsync("SUMMER_10", 100m));
        Assert.That(ex.Message, Is.EqualTo("Promo code has expired."));
    }

    [Test]
    public void ApplyDiscountAsync_ShouldThrowInvalidOperationException_WhenOrderAmountIsLessThanMinimum()
    {
        var promocode = new PromoCode{ Percentage = 10, IsActive = true, MaxDiscountAmount = 200m, ExpirationDate = DateTime.UtcNow.AddDays(1), MinOrderAmount = 100m };
        _repositoryMock.GetByCodeAsync("SUMMER_10").Returns(promocode);
        
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.ApplyDiscountAsync("SUMMER_10", 50m));
        Assert.That(ex.Message, Is.EqualTo("Order amount is below minimum discount amount."));
    }
    
    [Test]
    public async Task ApplyDiscountAsync_ShouldCapDiscount_WhenDiscountExceedsMaximum()
    {
        var promocode = new PromoCode{ Percentage = 10, IsActive = true, MaxDiscountAmount = 200m, ExpirationDate = DateTime.UtcNow.AddDays(1), MinOrderAmount = 100m };
        _repositoryMock.GetByCodeAsync("SUMMER_10").Returns(promocode);
        
        var result = await _service.ApplyDiscountAsync("SUMMER_10", 52000m);
        Assert.That(result, Is.EqualTo(51800m));
    }
    
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void ApplyDiscountAsync_ShouldThrowArgumentException_WhenPromoCodeIsIsNullOrWhiteSpace(string? invalidCode)
    {
        Assert.That(async () => await _service.ApplyDiscountAsync(invalidCode!, 100m), Throws.ArgumentException);
        _repositoryMock.DidNotReceiveWithAnyArgs().GetByCodeAsync(default);
    }

    [TestCase(0)]
    [TestCase(-50)]
    public void ApplyDiscountAsync_ShouldThrowArgumentOutOfRangeException_WhenOrderAmountIsZeroOrNegative(int invalidAmount)
    {
        Assert.That(async () => await _service.ApplyDiscountAsync("SUMMER_10", (decimal)invalidAmount), 
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [TestCase(0, 100)] 
    [TestCase(100, 0)] 
    public async Task ApplyDiscountAsync_ShouldCalculateCorrectly_WhenPercentageIsBoundaryValue(double percentage, double expectedPrice)
    {
        var promocode = new PromoCode 
        { 
            Percentage = (decimal)percentage, 
            IsActive = true, 
            ExpirationDate = DateTime.UtcNow.AddDays(1) 
        };
        _repositoryMock.GetByCodeAsync("BOUNDARY_PCT").Returns(promocode);

        var result = await _service.ApplyDiscountAsync("BOUNDARY_PCT", 100m);
    
        Assert.That(result, Is.EqualTo((decimal)expectedPrice));
    }
}