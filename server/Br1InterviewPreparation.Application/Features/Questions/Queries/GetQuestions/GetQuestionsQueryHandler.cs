using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsQueryHandler(IQuestionRepository repository)
    : IRequestHandler<GetQuestionsQuery, IEnumerable<QuestionSummaryDto>>
{
    public async Task<IEnumerable<QuestionSummaryDto>> Handle(GetQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        var questions = await repository.GetQuestionsWithAnswersAsync(
            request.CategoryId,
            request.Content,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return questions.Select(x => new QuestionSummaryDto
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            Content = x.Content,
            Hint = x.Hint,
            AnswersCount = x.Answers.Count,
        });
    }
}