namespace Sprint17_PromoCodeEngine.BusinessLogic;

public class PromoCode
{
    public string Code { get; set; }
    public decimal Percentage { get; set; }
    public decimal MinOrderAmount { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpirationDate { get; set; }
    
}