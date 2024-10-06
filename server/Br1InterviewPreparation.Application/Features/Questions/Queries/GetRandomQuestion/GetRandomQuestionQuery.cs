using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetRandomQuestion;

public class GetRandomQuestionQuery : IRequest<QuestionDto>
{
    public Guid? CategoryId { get; set; }
}
