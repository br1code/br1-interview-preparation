using Moq;
using Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;

namespace Br1InterviewPreparation.Tests.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandValidatorTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock;
    private readonly SubmitAnswerCommandValidator _validator;

    public SubmitAnswerCommandValidatorTests()
    {
        _questionRepositoryMock = new Mock<IQuestionRepository>();
        _validator = new SubmitAnswerCommandValidator(_questionRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        _questionRepositoryMock
            .Setup(r => r.QuestionExists(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = questionId,
            VideoFile = new FileUploadDto
            {
                FileName = "123.webm",
                ContentType = "video/webm",
                Content = [0x10, 0x10]
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_InvalidQuestionId_ReturnsFailure()
    {
        // Arrange
        _questionRepositoryMock
            .Setup(r => r.QuestionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.Empty,
            VideoFile = new FileUploadDto
            {
                FileName = "123.webm",
                ContentType = "video/webm",
                Content = [0x10, 0x10]
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(SubmitAnswerCommand.QuestionId) &&
            x.ErrorMessage == SubmitAnswerCommandValidator.EMPTY_QUESTION_ID_ERROR_MESSAGE);
    }

    [Fact]
    public async Task Validate_QuestionDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        _questionRepositoryMock
            .Setup(r => r.QuestionExists(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new SubmitAnswerCommand
        {
            QuestionId = questionId,
            VideoFile = new FileUploadDto
            {
                FileName = "123.webm",
                ContentType = "video/webm",
                Content = [0x10, 0x10]
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(SubmitAnswerCommand.QuestionId) &&
            x.ErrorMessage == SubmitAnswerCommandValidator.QUESTION_NOT_FOUND_ERROR_MESSAGE);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Validate_EmptyVideoFilename_ReturnsFailure(string videoFilename)
    {
        // Arrange
        _questionRepositoryMock
            .Setup(r => r.QuestionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.NewGuid(),
            VideoFile = new FileUploadDto
            {
                FileName = videoFilename,
                ContentType = "video/webm",
                Content = [0x10, 0x10]
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == $"{nameof(SubmitAnswerCommand.VideoFile)}.{nameof(FileUploadDto.FileName)}" &&
            x.ErrorMessage == SubmitAnswerCommandValidator.EMPTY_FILE_NAME_ERROR_MESSAGE);
    }

    [Fact]
    public async Task Validate_EmptyVideoContentType_ReturnsFailure()
    {
        // Arrange
        _questionRepositoryMock
            .Setup(r => r.QuestionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.NewGuid(),
            VideoFile = new FileUploadDto
            {
                FileName = "123.webm",
                ContentType = null,
                Content = [0x10, 0x10]
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == $"{nameof(SubmitAnswerCommand.VideoFile)}.{nameof(FileUploadDto.ContentType)}" &&
            x.ErrorMessage == SubmitAnswerCommandValidator.EMPTY_CONTENT_TYPE_ERROR_MESSAGE);
    }

    [Theory]
    [InlineData("text/plain")]
    [InlineData("asd")]
    [InlineData("")]
    public async Task Validate_InvalidVideoContentType_ReturnsFailure(string contentType)
    {
        // Arrange
        _questionRepositoryMock
            .Setup(r => r.QuestionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.NewGuid(),
            VideoFile = new FileUploadDto
            {
                FileName = "123.webm",
                ContentType = contentType,
                Content = [0x10, 0x10]
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == $"{nameof(SubmitAnswerCommand.VideoFile)}.{nameof(FileUploadDto.ContentType)}" &&
            x.ErrorMessage == SubmitAnswerCommandValidator.INVALID_CONTENT_TYPE_ERROR_MESSAGE);
    }

    [Fact]
    public async Task Validate_EmptyVideoContent_ReturnsFailure()
    {
        // Arrange
        _questionRepositoryMock
            .Setup(r => r.QuestionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.NewGuid(),
            VideoFile = new FileUploadDto
            {
                FileName = "123.webm",
                ContentType = "video/webm",
                Content = []
            }
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == $"{nameof(SubmitAnswerCommand.VideoFile)}.{nameof(FileUploadDto.Content)}" &&
            x.ErrorMessage == SubmitAnswerCommandValidator.EMPTY_CONTENT_ERROR_MESSAGE);
    }
}
