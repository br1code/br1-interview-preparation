using FluentValidation;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;

public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
{
    public const string CategoryEmptyErrorMessage = "Category is required.";
    public const string CategoryNotFoundErrorMessage = "Category does not exist.";
    public const string ContentEmptyErrorMessage = "Content is required.";

    private readonly ICategoryRepository _categoryRepository;

    public AddQuestionCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(CategoryEmptyErrorMessage);

        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExists)
            .WithMessage(CategoryNotFoundErrorMessage);

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(ContentEmptyErrorMessage);
    }

    private async Task<bool> CategoryExists(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _categoryRepository.CategoryExistsAsync(categoryId, cancellationToken);
    }
}
