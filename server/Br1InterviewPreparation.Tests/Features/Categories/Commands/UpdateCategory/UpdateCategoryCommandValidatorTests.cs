using Br1InterviewPreparation.Application.Features.Categories.Commands.UpdateCategory;

namespace Br1InterviewPreparation.Tests.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidatorTests
{
    private readonly UpdateCategoryCommandValidator _validator = new UpdateCategoryCommandValidator();

    [Fact]
    public async Task Validate_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new UpdateCategoryCommand
        {
            Id = Guid.NewGuid(),
            Name = "Databases",
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_InvalidName_ReturnsFailure(string name)
    {
        // Arrange
        var command = new UpdateCategoryCommand()
        {
            Id = Guid.NewGuid(),
            Name = name,
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(UpdateCategoryCommand.Name)
            && x.ErrorMessage == UpdateCategoryCommandValidator.NameEmptyErrorMessage);
    }
}