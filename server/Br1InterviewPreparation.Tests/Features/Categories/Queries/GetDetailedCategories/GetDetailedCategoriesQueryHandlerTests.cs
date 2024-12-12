using Moq;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetDetailedCategories;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Categories.Queries.GetDetailedCategories;

public class GetDetailedCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly GetDetailedCategoriesQueryHandler _handler;

    public GetDetailedCategoriesQueryHandlerTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _handler = new GetDetailedCategoriesQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsDetailedCategories()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var questions = new List<Question>
        {
            new() { CategoryId = categoryId, Content = "What is an index?" },
            new() { CategoryId = categoryId, Content = "What is a stored procedure?" },
        };
        var categories = new List<Category>
        {
            new() { Id = categoryId, Name = "Databases", Questions = questions },
        };

        _repositoryMock
            .Setup(r => r.GetDetailedCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);
        
        var query = new GetDetailedCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(categoryId, result.First().Id);
        Assert.Equal(2, result.First().QuestionsCount);
    }
}