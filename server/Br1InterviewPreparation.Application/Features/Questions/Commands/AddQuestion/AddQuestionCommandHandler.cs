using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;

public class AddQuestionCommandHandler(IQuestionRepository questionRepository) : IRequestHandler<AddQuestionCommand, Guid>
{
    public async Task<Guid> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question
        {
            CategoryId = request.CategoryId,
            Content = request.Content,
            Hint = request.Hint,
        };

        await questionRepository.AddQuestionAsync(question, cancellationToken);
        return question.Id;
    }
}
