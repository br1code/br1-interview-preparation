using Moq;
using Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;
using Br1InterviewPreparation.Application.Interfaces;

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
            .Setup(repo => repo.QuestionExists(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = questionId,
            VideoFilename = "123.mp4"
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
            .Setup(repo => repo.QuestionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.Empty,
            VideoFilename = "123.mp4"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(SubmitAnswerCommand.QuestionId) &&
            x.ErrorMessage == SubmitAnswerCommandValidator.QUESTION_ID_EMPTY_ERROR_MESSAGE);
    }

    [Fact]
    public async Task Validate_QuestionDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        _questionRepositoryMock
            .Setup(repo => repo.QuestionExists(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new SubmitAnswerCommand
        {
            QuestionId = questionId,
            VideoFilename = "123.mp4"
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
    public async Task Validate_InvalidVideoFilename_ReturnsFailure(string videoFilename)
    {
        // Arrange
        _questionRepositoryMock
            .Setup(repo => repo.QuestionExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new SubmitAnswerCommand
        {
            QuestionId = Guid.NewGuid(),
            VideoFilename = videoFilename
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x =>
            x.PropertyName == nameof(SubmitAnswerCommand.VideoFilename) &&
            x.ErrorMessage == SubmitAnswerCommandValidator.VIDEOFILENAME_EMPTY_ERROR_MESSAGE);
    }
}
