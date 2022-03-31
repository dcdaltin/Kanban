namespace Cards.Service.Validators;
using FluentValidation;
using Cards.Domain.Entities;

public class CardValidator : AbstractValidator<Card>
{
    public CardValidator()
    {
        RuleFor(c => c.List)
            .NotEmpty().WithMessage("Please enter the list.")
            .NotNull().WithMessage("Please enter the list.");

        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Please enter the title.")
            .NotNull().WithMessage("Please enter the title.");

        RuleFor(c => c.Content)
            .NotEmpty().WithMessage("Please enter the content.")
            .NotNull().WithMessage("Please enter the content.");
    }
}
