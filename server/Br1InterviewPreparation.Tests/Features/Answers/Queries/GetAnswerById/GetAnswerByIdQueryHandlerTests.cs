using Moq;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerById;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Tests.Features.Answers.Queries.GetAnswerById;

public class GetAnswerByIdQueryHandlerTests
{
    private readonly Mock<IAnswerRepository> _answerRepositoryMock;
    private readonly GetAnswerByIdQueryHandler _handler;

    public GetAnswerByIdQueryHandlerTests()
    {
        _answerRepositoryMock = new Mock<IAnswerRepository>();
        _handler = new GetAnswerByIdQueryHandler(_answerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_AnswerExists_ReturnsEntity()
    {
        // Arrange
        var answerId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answer = new Answer
        {
            Id = answerId,
            QuestionId = questionId,
            VideoFilename = "123.mp4"
        };

        _answerRepositoryMock
            .Setup(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var query = new GetAnswerByIdQuery { Id  = answerId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(answerId, result.Id);
        Assert.Equal(questionId, result.QuestionId);
        Assert.Equal("123.mp4", result.VideoFilename);
        _answerRepositoryMock
            .Verify(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AnswerDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var answerId = Guid.NewGuid();

        _answerRepositoryMock
            .Setup(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var query = new GetAnswerByIdQuery { Id = answerId };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        _answerRepositoryMock.Verify(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
