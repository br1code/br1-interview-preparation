using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetRandomQuestion;

public class GetRandomQuestionQueryHandler(IQuestionRepository repository) : IRequestHandler<GetRandomQuestionQuery, QuestionDto>
{
    public async Task<QuestionDto> Handle(GetRandomQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await repository.GetRandomQuestionAsync(request.CategoryId, cancellationToken);

        if (question is null)
        {
            throw new NotFoundException("No questions were found.");
        }

        return new QuestionDto
        {
            Id = question.Id,
            CategoryId = question.CategoryId,
            Content = question.Content,
            Hint = question.Hint,
        };
    }
}
