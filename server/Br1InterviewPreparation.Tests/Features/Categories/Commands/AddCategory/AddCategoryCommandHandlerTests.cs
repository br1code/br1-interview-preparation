using Moq;
using Br1InterviewPreparation.Application.Features.Categories.Commands.AddCategory;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Categories.Commands.AddCategory;

public class AddCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly AddCategoryCommandHandler _handler;

    public AddCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new AddCategoryCommandHandler(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddCategory()
    {
        // Arrange
        var newCategoryId = Guid.NewGuid();
        
        _categoryRepositoryMock
            .Setup(r => r.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((c, ct) => c.Id = newCategoryId)
            .Returns(Task.CompletedTask);

        var command = new AddCategoryCommand
        {
            Name = "Databases"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Result
        Assert.Equal(newCategoryId, result);
        _categoryRepositoryMock
            .Verify(r => r.AddCategoryAsync(
                It.Is<Category>(c => c.Name == command.Name), 
                It.IsAny<CancellationToken>())
            , Times.Once);
    }
}