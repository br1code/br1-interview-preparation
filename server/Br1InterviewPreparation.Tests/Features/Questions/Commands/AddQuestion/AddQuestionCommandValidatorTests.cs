﻿using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands.AddQuestion;

public class AddQuestionCommandValidatorTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly AddQuestionCommandValidator _validator;

    public AddQuestionCommandValidatorTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _validator = new AddQuestionCommandValidator(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock
            .Setup(r => r.CategoryExistsAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new AddQuestionCommand
        {
            CategoryId = categoryId,
            Content = "What is an index?"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_InvalidCategoryId_ReturnsFailure()
    {
        // Arrange
        _categoryRepositoryMock
            .Setup(r => r.CategoryExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new AddQuestionCommand
        {
            CategoryId = Guid.Empty,
            Content = "What is an index?"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(AddQuestionCommand.CategoryId) &&
            x.ErrorMessage == AddQuestionCommandValidator.CategoryEmptyErrorMessage);
    }

    [Fact]
    public async Task Validate_CategoryDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock
            .Setup(r => r.CategoryExistsAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new AddQuestionCommand
        {
            CategoryId = categoryId,
            Content = "What is an index?"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(AddQuestionCommand.CategoryId) &&
            x.ErrorMessage == AddQuestionCommandValidator.CategoryNotFoundErrorMessage);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_InvalidContent_ReturnsFailure(string content)
    {
        // Arrange
        _categoryRepositoryMock
            .Setup(r => r.CategoryExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new AddQuestionCommand
        {
            CategoryId = Guid.NewGuid(),
            Content = content
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(AddQuestionCommand.Content)
            && x.ErrorMessage == AddQuestionCommandValidator.ContentEmptyErrorMessage);
    }
}
