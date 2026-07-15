using FluentValidation;
using sprint19_MinimalAPI.Models.DTOs;

namespace sprint19_MinimalAPI.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price)
            .GreaterThan(0);
        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
    }
}