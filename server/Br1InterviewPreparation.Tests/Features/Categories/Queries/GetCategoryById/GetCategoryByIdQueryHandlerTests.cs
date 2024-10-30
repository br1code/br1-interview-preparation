using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategoryById;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Moq;

namespace Br1InterviewPreparation.Tests.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly GetCategoryByIdQueryHandler _handler;

    public GetCategoryByIdQueryHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetCategoryByIdQueryHandler(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryExists_ReturnsEntity()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new Category { Id = categoryId, Name = "Databases" };

        _categoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var query = new GetCategoryByIdQuery { Id = categoryId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
        Assert.Equal(category.Name, result.Name);
        _categoryRepositoryMock.Verify(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryDoesNotExists_ThrowsNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _categoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var query = new GetCategoryByIdQuery { Id = categoryId };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        _categoryRepositoryMock.Verify(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
