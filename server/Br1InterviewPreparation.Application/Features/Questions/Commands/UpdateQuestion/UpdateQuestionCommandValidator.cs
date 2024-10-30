using FluentValidation;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public const string CategoryIdEmptyErrorMessage = "Category Id is required.";
    public const string ContentEmptyErrorMessage = "Content is required.";
    public const string CategoryNotFoundErrorMessage = "Category does not exist.";

    private readonly ICategoryRepository _categoryRepository;

    public UpdateQuestionCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(CategoryIdEmptyErrorMessage);

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
