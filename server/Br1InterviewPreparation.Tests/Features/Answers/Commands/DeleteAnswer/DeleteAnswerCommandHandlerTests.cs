using Moq;
using MediatR;
using Br1InterviewPreparation.Application.Features.Answers.Commands.DeleteAnswer;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Tests.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandHandlerTests
{
    private readonly Mock<IAnswerRepository> _answerRepositoryMock;
    private readonly Mock<IVideoStorageService> _videoStorageServiceMock;
    private readonly DeleteAnswerCommandHandler _handler;

    public DeleteAnswerCommandHandlerTests()
    {
        _answerRepositoryMock = new Mock<IAnswerRepository>();
        _videoStorageServiceMock = new Mock<IVideoStorageService>();
        _handler = new DeleteAnswerCommandHandler(_answerRepositoryMock.Object, _videoStorageServiceMock.Object);
    }

    [Fact]
    public async Task Handle_AnswerExists_DeletesAnswer()
    {
        // Arrange
        var answerId = Guid.NewGuid();
        var answer = new Answer
        {
            Id = answerId,
            QuestionId = Guid.NewGuid(),
            VideoFilename = "123.webm",
        };

        _answerRepositoryMock
            .Setup(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var command = new DeleteAnswerCommand { Id = answerId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _answerRepositoryMock.Verify(r => r.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()), Times.Once);
        _videoStorageServiceMock.Verify(s => s.DeleteVideoFile(answer.VideoFilename), Times.Once);
    }

    [Fact]
    public async Task Handle_AnswerDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var answerId = Guid.NewGuid();

        _answerRepositoryMock
            .Setup(repo => repo.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var command = new DeleteAnswerCommand { Id = answerId };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _answerRepositoryMock.Verify(r => r.GetAnswerByIdAsync(answerId, It.IsAny<CancellationToken>()), Times.Once);
        _videoStorageServiceMock.Verify(s => s.DeleteVideoFile(It.IsAny<string>()), Times.Never);
    }
}
