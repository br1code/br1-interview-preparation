using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQuery : IRequest<QuestionWithAnswersDto>
{
    public required Guid Id { get; set; }
}
