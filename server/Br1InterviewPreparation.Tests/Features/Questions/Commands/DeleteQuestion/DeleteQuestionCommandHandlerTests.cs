using Moq;
using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Commands.DeleteQuestion;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandlerTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock;
    private readonly Mock<IVideoStorageService> _videoStorageServiceMock;
    private readonly DeleteQuestionCommandHandler _handler;

    public DeleteQuestionCommandHandlerTests()
    {
        _questionRepositoryMock = new Mock<IQuestionRepository>();
        _videoStorageServiceMock = new Mock<IVideoStorageService>();
        _handler = new DeleteQuestionCommandHandler(_questionRepositoryMock.Object, _videoStorageServiceMock.Object);
    }

    [Fact]
    public async Task Handle_QuestionExists_DeletesQuestion()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        
        var question = new Question
        {
            Id = questionId,
            CategoryId = Guid.NewGuid(),
            Content = "What is an index?",
        };

        _questionRepositoryMock
            .Setup(r => r.GetQuestionWithAnswersByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var command = new DeleteQuestionCommand { Id = questionId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _questionRepositoryMock.Verify(r => r.DeleteQuestionAsync(question, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_QuestionExists_DeletesRelatedAnswerVideoFiles()
    {
        // Arrange
        var questionId = Guid.NewGuid();

        var answers = new List<Answer>
        {
            new() { QuestionId = questionId, VideoFilename = "video.webm" },
            new() { QuestionId = questionId, VideoFilename = "video.webm" },
        };
        
        var question = new Question
        {
            Id = questionId,
            CategoryId = Guid.NewGuid(),
            Content = "What is an index?",
            Answers = answers, 
        };
        
        _questionRepositoryMock
            .Setup(r => r.GetQuestionWithAnswersByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var command = new DeleteQuestionCommand { Id = questionId };
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(Unit.Value, result);
        _videoStorageServiceMock.Verify((s => s.DeleteVideoFile(It.IsAny<string>())), Times.Exactly(answers.Count));
    }

    [Fact]
    public async Task Handle_QuestionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var questionId = Guid.NewGuid();

        _questionRepositoryMock
            .Setup(r => r.GetQuestionWithAnswersByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var command = new DeleteQuestionCommand
        {
            Id = questionId
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        
        _questionRepositoryMock.Verify(r => r.DeleteQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()), Times.Never);
        _videoStorageServiceMock.Verify(s => s.DeleteVideoFile(It.IsAny<string>()), Times.Never);
    }
}
