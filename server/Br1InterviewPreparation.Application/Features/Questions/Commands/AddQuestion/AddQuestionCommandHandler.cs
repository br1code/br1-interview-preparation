using MediatR;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AddQuestionCommandHandler(IQuestionRepository questionRepository, ICategoryRepository categoryRepository)
    {
        _questionRepository = questionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var question = new Question
        {
            CategoryId = category.Id,
            Content = request.Content,
            Hint = request.Hint,
        };

        await _questionRepository.AddQuestionAsync(question, cancellationToken);
        return question.Id;
    }
}
