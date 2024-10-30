using Moq;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Features.Categories.Commands.UpdateCategory;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly UpdateCategoryCommandHandler _handler;

    public UpdateCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new UpdateCategoryCommandHandler(_categoryRepositoryMock.Object);
    }
    
    [Fact]
    public async Task Handle_ValidCommand_UpdatesCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var category = new Category()
        {
            Id = categoryId,
            Name = "Databases"
        };

        _categoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var command = new UpdateCategoryCommand()
        {
            Id = categoryId,
            Name = "Design Patterns"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Id, result.Id);
        Assert.Equal(command.Name, result.Name);
        _categoryRepositoryMock
            .Verify(r => r.UpdateCategoryAsync(
                    It.Is<Category>(q => q.Id == categoryId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Fact]
    public async Task Handle_CategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _categoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var command = new UpdateCategoryCommand()
        {
            Id = categoryId,
            Name = "Design Patterns"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _categoryRepositoryMock
            .Verify(r => r.UpdateCategoryAsync(
                    It.Is<Category>(q => q.Id == categoryId),
                    It.IsAny<CancellationToken>()),
                Times.Never);
    }
}