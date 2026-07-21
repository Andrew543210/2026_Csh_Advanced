using FluentValidation;
using Microsoft.Extensions.Options;
using Sprint20_FluentValidation.Helpers;
using Sprint20_FluentValidation.Models;

namespace Sprint20_FluentValidation.Validators;

public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
{
    private readonly int _minimumAge;

    public UserRegistrationDtoValidator(IOptions<ValidationSettings> options)
    {
        _minimumAge = options.Value.MinimumAge;
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Please confirm your password")
            .Equal(x => x.Password).WithMessage("Passwords do not match");

        
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(ValidationHelpers.BeInPast)
                .WithMessage("Date of birth cannot be in the future")
            .Must(x => ValidationHelpers.BeValidAge(x, _minimumAge))
                .WithMessage($"You must be at least {_minimumAge} years old to register");
        
        RuleFor(x => x.PersonalInfo)
            .NotNull().WithMessage("Personal information is required")
            .SetValidator(new PersonalInfoValidator());

       
        RuleFor(x => x.Address)
            .NotNull().WithMessage("Address information is required")
            .SetValidator(new AddressInfoValidator());
    }
}