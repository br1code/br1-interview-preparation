using FluentValidation;

namespace Br1InterviewPreparation.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public const string NameEmptyErrorMessage = "Name is required.";

    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(NameEmptyErrorMessage);
    }
}