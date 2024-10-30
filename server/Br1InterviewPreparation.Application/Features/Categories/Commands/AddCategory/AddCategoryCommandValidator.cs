using FluentValidation;

namespace Br1InterviewPreparation.Application.Features.Categories.Commands.AddCategory;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public const string NameEmptyErrorMessage = "Name is required.";

    public AddCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(NameEmptyErrorMessage);
    }
}