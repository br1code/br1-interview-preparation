using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsQuery : IRequest<IEnumerable<QuestionDto>>
{
    public Guid? CategoryId { get; set; }
}
