using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    private readonly IQuestionRepository _questionRepository;

    public AddQuestionCommandHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Guid> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question
        {
            CategoryId = request.CategoryId,
            Content = request.Content,
            Hint = request.Hint,
        };

        await _questionRepository.AddQuestionAsync(question, cancellationToken);
        return question.Id;
    }
}
