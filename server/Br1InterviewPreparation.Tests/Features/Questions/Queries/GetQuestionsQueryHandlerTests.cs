using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Questions.Queries;

public class GetQuestionsQueryHandlerTests
{
    private readonly Mock<IQuestionRepository> _repositoryMock;
    private readonly GetQuestionsQueryHandler _handler;

    public GetQuestionsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IQuestionRepository>();
        _handler = new GetQuestionsQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NoCategoryId_ReturnsAllQuestions()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var questions = new List<Question>
        {
            new Question { CategoryId = categoryId, Content = "What is an index?" },
            new Question { CategoryId = categoryId, Content = "What is sharding?" },
        };

        _repositoryMock
            .Setup(r => r.GetQuestionsAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        var query = new GetQuestionsQuery { CategoryId = null };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(r => r.GetQuestionsAsync(null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCategoryId_ReturnsFilteredQuestions()
    {
        // Arrange
        var category = new Category { Id = Guid.NewGuid(), Name = "Databases" };
        var questions = new List<Question>
        {
            new Question { CategoryId = category.Id, Content = "What is an index?" },
        };

        _repositoryMock
            .Setup(r => r.GetQuestionsAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        var query = new GetQuestionsQuery { CategoryId = category.Id };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("What is an index?", result.First().Content);
        _repositoryMock.Verify(r => r.GetQuestionsAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCategoryId_NoMatches_ReturnsEmptyList()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.GetQuestionsAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Question>());

        var query = new GetQuestionsQuery { CategoryId = categoryId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _repositoryMock.Verify(r => r.GetQuestionsAsync(categoryId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
