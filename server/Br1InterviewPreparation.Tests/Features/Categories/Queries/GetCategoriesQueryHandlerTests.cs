using Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategories;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Moq;

namespace Br1InterviewPreparation.Tests.Features.Categories.Queries;

public class GetCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly GetCategoriesQueryHandler _handler;

    public GetCategoriesQueryHandlerTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetCategoriesQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Databases" },
            new Category { Id = Guid.NewGuid(), Name = "Design Patterns" },
        };

        _repositoryMock
            .Setup(repo => repo.GetCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        var query = new GetCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(repo => repo.GetCategoriesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
