using FluentValidation;
using Sprint20_FluentValidation.Models;

namespace Sprint20_FluentValidation.Validators;

public class AddressInfoValidator : AbstractValidator<AddressInfoDto>
{
    public AddressInfoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required")
            .MaximumLength(100);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(50);

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip code is required")
            .Matches(@"^\d{5}(-\d{4})?$").WithMessage("Invalid zip code format");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(50);
    }
}