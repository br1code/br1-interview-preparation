using Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Moq;

namespace Br1InterviewPreparation.Tests.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandHandlerTests
{
    private readonly Mock<IAnswerRepository> _answerRepositoryMock;
    private readonly Mock<IVideoStorageService> _videoStorageServiceMock;
    private readonly SubmitAnswerCommandHandler _handler;

    public SubmitAnswerCommandHandlerTests()
    {
        _answerRepositoryMock = new Mock<IAnswerRepository>();
        _videoStorageServiceMock = new Mock<IVideoStorageService>();
        _handler = new SubmitAnswerCommandHandler(_answerRepositoryMock.Object, _videoStorageServiceMock.Object);
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

        var fileName = "123.webm";
        var contentType = "video/webm";
        byte[] content = [0x10, 0x10];
        var filePath = "/videos/123.webm";

        _videoStorageServiceMock
            .Setup(s => s.SaveVideoFileAsync(fileName, contentType, content, It.IsAny<CancellationToken>()))
            .ReturnsAsync(filePath);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.NewGuid(),
            VideoFile = new FileUploadDto
            {
                FileName = fileName,
                ContentType = contentType,
                Content = content
            }
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(newAnswerId, result);
        _answerRepositoryMock
            .Verify(r => r.AddAnswerAsync(
                It.Is<Answer>(a => a.QuestionId == command.QuestionId && a.VideoFilename == filePath),
                It.IsAny<CancellationToken>())
            , Times.Once);
        _videoStorageServiceMock.Verify(s => s.SaveVideoFileAsync(fileName, contentType, content, It.IsAny<CancellationToken>()), Times.Once);
    }
}
