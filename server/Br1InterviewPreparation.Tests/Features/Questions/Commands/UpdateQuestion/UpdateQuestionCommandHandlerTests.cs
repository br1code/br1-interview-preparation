using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Commands.UpdateQuestion;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandlerTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock;
    private readonly UpdateQuestionCommandHandler _handler;

    public UpdateQuestionCommandHandlerTests()
    {
        _questionRepositoryMock = new Mock<IQuestionRepository>();
        _handler = new UpdateQuestionCommandHandler(_questionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesQuestion()
    {
        // Arrange
        var questionId = Guid.NewGuid();

        var question = new Question
        {
            Id = questionId,
            CategoryId = Guid.NewGuid(),
            Content = "What is an index?",
            Hint = "Trees ..."
        };

        _questionRepositoryMock
            .Setup(r => r.GetQuestionByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var command = new UpdateQuestionCommand
        {
            Id = questionId,
            CategoryId = Guid.NewGuid(),
            Content = "What is sharding?",
            Hint = "Help ..."
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Id, result.Id);
        Assert.Equal(command.CategoryId, result.CategoryId);
        Assert.Equal(command.Content, result.Content);
        Assert.Equal(command.Hint, result.Hint);
        _questionRepositoryMock
            .Verify(r => r.UpdateQuestionAsync(
                It.Is<Question>(q => q.Id == questionId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_QuestionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var questionId = Guid.NewGuid();

        _questionRepositoryMock
            .Setup(r => r.GetQuestionByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var command = new UpdateQuestionCommand
        {
            Id = questionId,
            CategoryId = Guid.NewGuid(),
            Content = "What is sharding?",
            Hint = "Help ..."
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _questionRepositoryMock
            .Verify(r => r.UpdateQuestionAsync(
                It.Is<Question>(q => q.Id == questionId),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
