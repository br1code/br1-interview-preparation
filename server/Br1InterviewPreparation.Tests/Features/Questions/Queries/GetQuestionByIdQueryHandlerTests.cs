using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestionById;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Tests.Features.Questions.Queries;

public class GetQuestionByIdQueryHandlerTests
{
    private readonly Mock<IQuestionRepository> _repositoryMock;
    private readonly GetQuestionByIdQueryHandler _handler;

    public GetQuestionByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IQuestionRepository>();
        _handler = new GetQuestionByIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_QuestionExists_ReturnsQuestionWithAnswers()
    {
        // Arrange
        var category = new Category { Name = "Databases" };
        var questionId = Guid.NewGuid();
        var question = new Question
        {
            Id = questionId,
            CategoryId = category.Id,
            Content = "What is an index?",
        };
        var answer = new Answer 
        { 
            Id = Guid.NewGuid(), 
            QuestionId = questionId, 
            VideoFilename = "123.mp4",
            CreatedAt = DateTime.Now,
        };
        question.Answers.Add(answer);

        _repositoryMock
            .Setup(r => r.GetQuestionWithAnswersByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var query = new GetQuestionByIdQuery { Id = questionId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(questionId, result.Id);
        Assert.Equal(question.Content, result.Content);
        Assert.Equal(question.CategoryId, result.CategoryId);
        Assert.Single(question.Answers);
        Assert.Equal(answer.Id, question.Answers.First().Id);
    }

    [Fact]
    public async Task Handle_QuestionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var questionId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetQuestionWithAnswersByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var query = new GetQuestionByIdQuery { Id = questionId };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
