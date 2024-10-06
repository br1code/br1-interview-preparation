﻿using Moq;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetRandomQuestion;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Questions.Queries;

public class GetRandomQuestionQueryHandlerTests
{
    private readonly Mock<IQuestionRepository> _repositoryMock;
    private readonly GetRandomQuestionQueryHandler _handler;

    public GetRandomQuestionQueryHandlerTests()
    {
        _repositoryMock = new Mock<IQuestionRepository>();
        _handler = new GetRandomQuestionQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NoCategoryId_ReturnsRandomQuestion()
    {
        // Arrange
        var category = new Category { Name = "Databases" };
        var question = new Question { Id = Guid.NewGuid(), Category = category, Content = "What is an index?" };

        _repositoryMock
            .Setup(repo => repo.GetRandomQuestionAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var query = new GetRandomQuestionQuery { CategoryId = null };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(question.Id, result.Id);
        Assert.Equal(question.Content, result.Content);
        Assert.Equal(question.CategoryId, result.CategoryId);
        _repositoryMock.Verify(repo => repo.GetRandomQuestionAsync(null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCategoryId_ReturnsRandomQuestionFromCategory()
    {
        // Arrange
        var category = new Category { Name = "Databases" };
        var question = new Question { Category = category, Content = "What is an index?" };

        _repositoryMock
            .Setup(repo => repo.GetRandomQuestionAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var query = new GetRandomQuestionQuery { CategoryId = category.Id };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(question.Id, result.Id);
        Assert.Equal(question.Content, result.Content);
        Assert.Equal(question.CategoryId, result.CategoryId);
        _repositoryMock.Verify(repo => repo.GetRandomQuestionAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NoQuestionsFound_ThrowsNotFoundException()
    {
        // Arrange
        _repositoryMock
            .Setup(repo => repo.GetRandomQuestionAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var query = new GetRandomQuestionQuery { CategoryId = null };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        _repositoryMock.Verify(repo => repo.GetRandomQuestionAsync(null, It.IsAny<CancellationToken>()), Times.Once);
    }
}
