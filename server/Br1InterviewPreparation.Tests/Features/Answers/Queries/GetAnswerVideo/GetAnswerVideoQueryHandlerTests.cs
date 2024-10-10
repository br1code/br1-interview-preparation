using Moq;
using Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerVideo;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Answers.Queries.GetAnswerVideo;

public class GetAnswerVideoQueryHandlerTests
{
    private readonly Mock<IAnswerRepository> _answerRepositoryMock;
    private readonly GetAnswerVideoQueryHandler _handler;

    public GetAnswerVideoQueryHandlerTests()
    {
        _answerRepositoryMock = new Mock<IAnswerRepository>();
        _handler = new GetAnswerVideoQueryHandler(_answerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_AnswerExists_ReturnsFilePath()
    {
        // Arrange
        var answerId = Guid.NewGuid();
        var answer = new Answer
        {
            Id = answerId,
            QuestionId = Guid.NewGuid(),
            VideoFilename = "123.webm"
        };

        _answerRepositoryMock
            .Setup(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var query = new GetAnswerVideoQuery { Id = answerId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        var expectedFilePath = $"/videos/{answer.VideoFilename}";
        var expectedContentType = "video/webm";
        Assert.Equal(expectedFilePath, result.FilePath);
        Assert.Equal(expectedContentType, result.ContentType);

        _answerRepositoryMock
            .Verify(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AnswerDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var answerId = Guid.NewGuid();
        var query = new GetAnswerVideoQuery { Id = answerId };

        _answerRepositoryMock
            .Setup(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
