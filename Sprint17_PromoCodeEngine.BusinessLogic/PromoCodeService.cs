namespace Sprint17_PromoCodeEngine.BusinessLogic;

public class PromoCodeService(IPromoCodeRepository repository)
{
    public async Task<decimal> ApplyDiscountAsync(string code, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Promo code cannot be null, empty or whitespace.", nameof(code));
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Order amount must be greater than zero.");
        }

        var promoCode = await repository.GetByCodeAsync(code);
        
        if (promoCode == null)
        {
            throw new ArgumentException("Invalid promo code");
        }
        
        if (!promoCode.IsActive)
        {
            throw new InvalidOperationException("Promo code is inactive.");
        }
        
        if (promoCode.ExpirationDate < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Promo code has expired.");
        }
        
        if (promoCode.MinOrderAmount > amount)
        {
            throw new InvalidOperationException("Order amount is below minimum discount amount.");
        }
        
        var discount = amount * promoCode.Percentage / 100;

        if (promoCode.MaxDiscountAmount > 0 && discount > promoCode.MaxDiscountAmount)
        {
            discount = promoCode.MaxDiscountAmount;
        }

        return amount - discount;
    }
}