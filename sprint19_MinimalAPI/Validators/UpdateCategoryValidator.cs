using FluentValidation;
using sprint19_MinimalAPI.Models.DTOs;

namespace sprint19_MinimalAPI.Validators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description)
            .MaximumLength(200).When(x => x.Description is not null);
    }
}