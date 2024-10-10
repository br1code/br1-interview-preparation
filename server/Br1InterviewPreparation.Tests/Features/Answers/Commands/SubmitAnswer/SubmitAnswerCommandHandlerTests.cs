using Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Moq;

namespace Br1InterviewPreparation.Tests.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandHandlerTests
{
    private readonly Mock<IAnswerRepository> _answerRepositoryMock;
    private readonly SubmitAnswerCommandHandler _handler;

    public SubmitAnswerCommandHandlerTests()
    {
        _answerRepositoryMock = new Mock<IAnswerRepository>();
        _handler = new SubmitAnswerCommandHandler(_answerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsAnswer()
    {
        // Arrange
        var newAnswerId = Guid.NewGuid();

        _answerRepositoryMock
            .Setup(r => r.AddAnswerAsync(It.IsAny<Answer>(), It.IsAny<CancellationToken>()))
            .Callback<Answer, CancellationToken>((a, ct) => a.Id = newAnswerId)
            .Returns(Task.CompletedTask);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.NewGuid(),
            VideoFilename = "123.mp4"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(newAnswerId, result);
        _answerRepositoryMock
            .Verify(r => r.AddAnswerAsync(
                It.Is<Answer>(a => a.QuestionId == command.QuestionId && a.VideoFilename == command.VideoFilename),
                It.IsAny<CancellationToken>())
            , Times.Once);
    }
}
