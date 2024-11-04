using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Questions.Queries;

public class GetQuestionsQueryHandlerTests
{
    private readonly Mock<IQuestionRepository> _repositoryMock;
    private readonly GetQuestionsQueryHandler _handler;

    private readonly Guid _defaultCategoryId;
    private readonly int _defaultPageNumber;
    private readonly int? _defaultPageSize;
    private readonly string? _defaultContent;

    public GetQuestionsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IQuestionRepository>();
        _handler = new GetQuestionsQueryHandler(_repositoryMock.Object);

        _defaultCategoryId = Guid.NewGuid();
        _defaultPageNumber = 1;
        _defaultPageSize = null;
        _defaultContent = null;
    }

    [Fact]
    public async Task Handle_NoCategoryId_ReturnsAllQuestions()
    {
        // Arrange
        var questions = new List<Question>
        {
            new() { CategoryId = _defaultCategoryId, Content = "What is an index?" },
            new() { CategoryId = _defaultCategoryId, Content = "What is sharding?" },
        };

        _repositoryMock
            .Setup(r => r.GetQuestionsWithAnswersAsync(null, _defaultContent, _defaultPageNumber, _defaultPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        var query = new GetQuestionsQuery
            { CategoryId = null, PageNumber = _defaultPageNumber, PageSize = _defaultPageSize };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(
            r => r.GetQuestionsWithAnswersAsync(null, _defaultContent, _defaultPageNumber, _defaultPageSize,
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCategoryId_ReturnsFilteredQuestions()
    {
        // Arrange
        var questions = new List<Question>
        {
            new() { CategoryId = _defaultCategoryId, Content = "What is an index?" },
        };

        _repositoryMock
            .Setup(r => r.GetQuestionsWithAnswersAsync(_defaultCategoryId, _defaultContent, _defaultPageNumber,
                _defaultPageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        var query = new GetQuestionsQuery
            { CategoryId = _defaultCategoryId, PageNumber = _defaultPageNumber, PageSize = _defaultPageSize };

        // Act
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("What is an index?", result.First().Content);
        _repositoryMock.Verify(
            r => r.GetQuestionsWithAnswersAsync(_defaultCategoryId, _defaultContent, _defaultPageNumber,
                _defaultPageSize, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCategoryId_NoMatches_ReturnsEmptyList()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetQuestionsWithAnswersAsync(_defaultCategoryId, _defaultContent, _defaultPageNumber,
                _defaultPageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var query = new GetQuestionsQuery
            { CategoryId = _defaultCategoryId, PageNumber = _defaultPageNumber, PageSize = _defaultPageSize };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _repositoryMock.Verify(
            r => r.GetQuestionsWithAnswersAsync(_defaultCategoryId, _defaultContent, _defaultPageNumber,
                _defaultPageSize, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithPagination_ReturnsPagedQuestions()
    {
        // Arrange
        const int pageNumber = 2;
        const int pageSize = 1;

        var questions = new List<Question>
        {
            new() { CategoryId = _defaultCategoryId, Content = "What is an index?" },
            new() { CategoryId = _defaultCategoryId, Content = "What is sharding?" },
        };

        _repositoryMock
            .Setup(r => r.GetQuestionsWithAnswersAsync(null, _defaultContent, pageNumber, pageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions.Skip(1).Take(1).ToList());

        var query = new GetQuestionsQuery { CategoryId = null, PageNumber = pageNumber, PageSize = pageSize };

        // Act
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("What is sharding?", result.First().Content);
        _repositoryMock.Verify(
            r => r.GetQuestionsWithAnswersAsync(null, _defaultContent, pageNumber, pageSize,
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithContentFilter_ReturnsMatchingQuestions()
    {
        // Arrange
        const string contentFilter = "index";

        var questions = new List<Question>
        {
            new() { CategoryId = _defaultCategoryId, Content = "What is an index?" },
            new() { CategoryId = _defaultCategoryId, Content = "What is sharding?" },
        };

        _repositoryMock
            .Setup(r => r.GetQuestionsWithAnswersAsync(null, contentFilter, _defaultPageNumber, _defaultPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions.Where(q => q.Content.ToLower().Contains(contentFilter.ToLower())).ToList());

        var query = new GetQuestionsQuery
        {
            CategoryId = null, Content = contentFilter, PageNumber = _defaultPageNumber, PageSize = _defaultPageSize
        };

        // Act
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("What is an index?", result.First().Content);
        _repositoryMock.Verify(
            r => r.GetQuestionsWithAnswersAsync(null, contentFilter, _defaultPageNumber, _defaultPageSize,
                It.IsAny<CancellationToken>()), Times.Once);
    }
}