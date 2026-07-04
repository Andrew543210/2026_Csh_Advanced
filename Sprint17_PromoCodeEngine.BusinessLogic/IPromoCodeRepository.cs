    namespace Sprint17_PromoCodeEngine.BusinessLogic;

    public interface IPromoCodeRepository
    { 
        Task<PromoCode?> GetByCodeAsync(string code);
    }