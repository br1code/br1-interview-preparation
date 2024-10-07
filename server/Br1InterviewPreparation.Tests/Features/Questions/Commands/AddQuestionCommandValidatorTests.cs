using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands;

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
            .Setup(repo => repo.CategoryExistsAsync(categoryId, It.IsAny<CancellationToken>()))
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
    public async Task Validate_InvalidCommand_ReturnsFailure()
    {
        // Arrange
        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new AddQuestionCommand
        {
            CategoryId = Guid.Empty,
            Content = string.Empty
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => 
            x.PropertyName == nameof(AddQuestionCommand.CategoryId) && 
            x.ErrorMessage == AddQuestionCommandValidator.CATEGORY_EMPTY_ERROR_MESSAGE);
        Assert.Contains(result.Errors, x => 
            x.PropertyName == nameof(AddQuestionCommand.Content) 
            && x.ErrorMessage == AddQuestionCommandValidator.CONTENT_EMPTY_ERROR_MESSAGE);
    }

    [Fact]
    public async Task Validate_CategoryNotFound_ReturnsFailure()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(categoryId, It.IsAny<CancellationToken>()))
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
            x.ErrorMessage == AddQuestionCommandValidator.CATEGORY_NOT_FOUND_ERROR_MESSAGE);
    }
}
