using FluentValidation;
using sprint19_MinimalAPI.Models.DTOs;

namespace sprint19_MinimalAPI.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description)
            .MaximumLength(200).When(x => x.Description is not null);
    }
}