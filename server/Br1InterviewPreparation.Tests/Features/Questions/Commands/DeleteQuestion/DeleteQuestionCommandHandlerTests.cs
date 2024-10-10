using Moq;
using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Commands.DeleteQuestion;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandlerTests
{
    private readonly Mock<IQuestionRepository> _questionRepository;
    private readonly DeleteQuestionCommandHandler _handler;

    public DeleteQuestionCommandHandlerTests()
    {
        _questionRepository = new Mock<IQuestionRepository>();
        _handler = new DeleteQuestionCommandHandler(_questionRepository.Object);
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
            Content = "What is an index?"
        };

        _questionRepository
            .Setup(repo => repo.GetQuestionByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var command = new DeleteQuestionCommand { Id = questionId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _questionRepository.Verify(repo => repo.GetQuestionByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _questionRepository.Verify(repo => repo.DeleteQuestionAsync(question, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_QuestionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var questionId = Guid.NewGuid();

        _questionRepository
            .Setup(repo => repo.GetQuestionByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var command = new DeleteQuestionCommand
        {
            Id = questionId
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _questionRepository.Verify(repo => repo.DeleteQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
