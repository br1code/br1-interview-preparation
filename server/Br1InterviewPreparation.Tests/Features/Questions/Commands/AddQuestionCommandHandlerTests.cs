using Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Moq;

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

    // TODO:
    //[Fact]
    //public async Task Handle_ValidCommand_AddsQuestion()
    //{
    //    // Arrange
    //    var category = new Category { Id = Guid.NewGuid(), Name = "Databases" };

    //    var command = new AddQuestionCommand
    //    {
    //        CategoryId = category.Id,
    //        Content = "What is an index?"
    //    };

    //    var question = new Question
    //    {
    //        Category = category,
    //        Content = command.Content,
    //    };

    //    _categoryRepositoryMock
    //        .Setup(repo => repo.GetCategoryByIdAsync(category.Id, It.IsAny<CancellationToken>()))
    //        .ReturnsAsync(category);

    //    // Act

    //    // Assert
    //}
}
