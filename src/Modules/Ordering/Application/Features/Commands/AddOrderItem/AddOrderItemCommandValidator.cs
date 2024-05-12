using FluentValidation;

namespace Astrum.Ordering.Features.Commands.AddOrderItem;

public class AddOrderItemCommandValidator : AbstractValidator<AddOrderItemCommand>
{
    public AddOrderItemCommandValidator()
    {
        RuleFor(p => p.ProductName)
            .NotEmpty().WithMessage("{ProductName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{ProductName} must not exceed 50 characters.");

        RuleFor(p => p.ProductPrice)
            .NotEmpty().WithMessage("{ProductPrice} is required.")
            .GreaterThan(0).WithMessage("{ProductPrice} should be greater than zero.");
    }
}
