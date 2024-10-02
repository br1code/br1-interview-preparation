using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, IEnumerable<QuestionDto>>
{
    private readonly IQuestionRepository _repository;

    public GetQuestionsQueryHandler(IQuestionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await _repository.GetQuestionsAsync(request.CategoryId, cancellationToken);

        return questions.Select(x => new QuestionDto 
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            Content = x.Content,
            Hint = x.Hint,
        });
    }
}
