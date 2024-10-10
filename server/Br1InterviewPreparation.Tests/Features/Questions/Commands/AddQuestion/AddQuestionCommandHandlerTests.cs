using Moq;
using Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands.AddQuestion;

public class AddQuestionCommandHandlerTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock;
    private readonly AddQuestionCommandHandler _handler;

    public AddQuestionCommandHandlerTests()
    {
        _questionRepositoryMock = new Mock<IQuestionRepository>();
        _handler = new AddQuestionCommandHandler(_questionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsQuestion()
    {
        // Arrange
        var newQuestionId = Guid.NewGuid();

        _questionRepositoryMock
            .Setup(r => r.AddQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
            .Callback<Question, CancellationToken>((q, ct) => q.Id = newQuestionId)
            .Returns(Task.CompletedTask);

        var command = new AddQuestionCommand
        {
            CategoryId = Guid.NewGuid(),
            Content = "What is an index?",
            Hint = "Trees ..."
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(newQuestionId, result);
        _questionRepositoryMock
            .Verify(r => r.AddQuestionAsync(
                It.Is<Question>(q => q.CategoryId == command.CategoryId && q.Content == command.Content && q.Hint == command.Hint),
                It.IsAny<CancellationToken>())
            , Times.Once);
    }
}
