using Br1InterviewPreparation.Application.Features.Categories.Commands.AddCategory;

namespace Br1InterviewPreparation.Tests.Features.Categories.Commands.AddCategory;

public class AddCategoryCommandValidatorTests
{
    private readonly AddCategoryCommandValidator _validator = new();

    [Fact]
    public async Task Validate_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            Name = "Databases"
        };
        
        // Act
        var result = await _validator.ValidateAsync(command);
        
        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_EmptyName_ReturnsFailure(string name)
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            Name = name
        };
        
        // Act
        var result = await _validator.ValidateAsync(command);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(AddCategoryCommand.Name) &&
            x.ErrorMessage == AddCategoryCommandValidator.NameEmptyErrorMessage);
    }
}