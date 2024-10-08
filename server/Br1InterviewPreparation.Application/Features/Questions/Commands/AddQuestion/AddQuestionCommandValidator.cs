﻿using FluentValidation;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;

public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
{
    public const string CATEGORY_EMPTY_ERROR_MESSAGE = "Category is required.";
    public const string CATEGORY_NOT_FOUND_ERROR_MESSAGE = "Category does not exist.";
    public const string CONTENT_EMPTY_ERROR_MESSAGE = "Content is required.";

    private readonly ICategoryRepository _categoryRepository;

    public AddQuestionCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(CATEGORY_EMPTY_ERROR_MESSAGE);

        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExists)
            .WithMessage(CATEGORY_NOT_FOUND_ERROR_MESSAGE);

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(CONTENT_EMPTY_ERROR_MESSAGE);
    }

    private async Task<bool> CategoryExists(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _categoryRepository.CategoryExistsAsync(categoryId, cancellationToken);
    }
}
