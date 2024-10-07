using Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands;

public class AddQuestionCommandValidatorTests
{
    private readonly AddQuestionCommandValidator _validator;

    public AddQuestionCommandValidatorTests()
    {
        _validator = new AddQuestionCommandValidator();
    }

    [Fact]
    public async Task Validate_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new AddQuestionCommand
        {
            CategoryId = Guid.NewGuid(),
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
        var command = new AddQuestionCommand
        {
            CategoryId = Guid.Empty,
            Content = string.Empty
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(AddQuestionCommand.CategoryId));
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(AddQuestionCommand.Content));
    }
}
