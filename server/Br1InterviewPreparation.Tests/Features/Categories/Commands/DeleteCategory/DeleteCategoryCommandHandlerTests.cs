using Moq;
using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Commands.DeleteCategory;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Tests.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IVideoStorageService> _videoStorageServiceMock;
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _videoStorageServiceMock = new Mock<IVideoStorageService>();
        _handler = new DeleteCategoryCommandHandler(_categoryRepositoryMock.Object, _videoStorageServiceMock.Object);
    }

    [Fact]
    public async Task Handle_CategoryExists_DeletesCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var category = new Category
        {
            Id = categoryId,
            Name = "Databases"
        };
        
        _categoryRepositoryMock
            .Setup(r => r.GetCategoryWithQuestionsByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        
        var command = new DeleteCategoryCommand { Id = categoryId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _categoryRepositoryMock.Verify(r => r.GetCategoryWithQuestionsByIdAsync(categoryId, It.IsAny<CancellationToken>()), Times.Once);
        _categoryRepositoryMock.Verify(r => r.DeleteCategoryAsync(category, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryExists_DeletesAnswerVideoFilesOfRelatedQuestions()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        
        var answers = new List<Answer>
        {
            new() { QuestionId = questionId, VideoFilename = "video.webm" },
            new() { QuestionId = questionId, VideoFilename = "video.webm" },
        };
        
        var categoryId = Guid.NewGuid();
        
        var question = new Question
        {
            Id = questionId,
            CategoryId = categoryId,
            Content = "What is an index?",
            Answers = answers, 
        };

        var category = new Category
        {
            Id = categoryId,
            Name = "Databases",
            Questions = new List<Question> { question }
        };
        
        _categoryRepositoryMock
            .Setup(r => r.GetCategoryWithQuestionsByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        
        var command = new DeleteCategoryCommand { Id = categoryId };
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(Unit.Value, result);
        _videoStorageServiceMock.Verify((s => s.DeleteVideoFile(It.IsAny<string>())), Times.Exactly(answers.Count));
    }
}