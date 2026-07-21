namespace Sprint20_FluentValidation.Helpers;

public static class ValidationHelpers
{
    public static bool BeInPast(DateTime dateOfBirth)
    {
        return dateOfBirth < DateTime.Today;
    }

    public static bool BeValidAge(DateTime dateOfBirth, int minimumAge)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        
        if (today.AddYears(-age) < dateOfBirth)
        {
            age--;
        }

        return age >= minimumAge;
    }
}