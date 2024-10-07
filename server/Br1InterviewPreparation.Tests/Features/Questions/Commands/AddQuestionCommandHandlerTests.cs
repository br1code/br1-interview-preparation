using Moq;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Questions.Commands;

public class AddQuestionCommandHandlerTests
{
    private readonly Mock<IQuestionRepository> _questionRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly AddQuestionCommandHandler _handler;

    public AddQuestionCommandHandlerTests()
    {
        _questionRepositoryMock = new Mock<IQuestionRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _handler = new AddQuestionCommandHandler(_questionRepositoryMock.Object, _categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsQuestion()
    {
        // Arrange
        var category = new Category { Id = Guid.NewGuid(), Name = "Databases" };

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var newQuestionId = Guid.NewGuid();

        _questionRepositoryMock
            .Setup(repo => repo.AddQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
            .Callback<Question, CancellationToken>((q, ct) => q.Id = newQuestionId)
            .Returns(Task.CompletedTask);

        var command = new AddQuestionCommand
        {
            CategoryId = category.Id,
            Content = "What is an index?",
            Hint = "Trees ..."
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(newQuestionId, result);
        _questionRepositoryMock
            .Verify(repo => repo.AddQuestionAsync(
                It.Is<Question>(q => q.CategoryId == command.CategoryId && q.Content == command.Content && q.Hint == command.Hint), 
                It.IsAny<CancellationToken>())
            , Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _categoryRepositoryMock
            .Setup(repo => repo.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        var command = new AddQuestionCommand
        {
            CategoryId = categoryId,
            Content = "What is an index?",
            Hint = "Trees ..."
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _questionRepositoryMock.Verify(repo => repo.AddQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
