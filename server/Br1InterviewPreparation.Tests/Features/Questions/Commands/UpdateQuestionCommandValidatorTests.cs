using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Commands.UpdateQuestion;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands;

public class UpdateQuestionCommandValidatorTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly UpdateQuestionCommandValidator _validator;

    public UpdateQuestionCommandValidatorTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _validator = new UpdateQuestionCommandValidator(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new UpdateQuestionCommand
        {
            QuestionId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            Content = "What is sharding?",
            Hint = "Some help ..."
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
        var command = new UpdateQuestionCommand
        {
            QuestionId = Guid.NewGuid(),
            CategoryId = Guid.Empty,
            Content = "What is sharding?",
            Hint = "Some help ..."
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(UpdateQuestionCommand.CategoryId)
            && x.ErrorMessage == UpdateQuestionCommandValidator.CATEGORY_ID_EMPTY_ERROR_MESSAGE);
    }

    [Fact]
    public async Task Validate_CategoryDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryRepositoryMock
            .Setup(repo => repo.CategoryExistsAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new UpdateQuestionCommand
        {
            QuestionId = Guid.NewGuid(),
            CategoryId = categoryId,
            Content = "What is sharding?",
            Hint = "Some help ..."
        };

        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(UpdateQuestionCommand.CategoryId) &&
            x.ErrorMessage == UpdateQuestionCommandValidator.CATEGORY_NOT_FOUND_ERROR_MESSAGE);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_InvalidContent_ReturnsFailure(string content)
    {
        // Arrange
        var command = new UpdateQuestionCommand
        {
            QuestionId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            Content = content
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(UpdateQuestionCommand.Content)
            && x.ErrorMessage == UpdateQuestionCommandValidator.CONTENT_EMPTY_ERROR_MESSAGE);
    }
}
