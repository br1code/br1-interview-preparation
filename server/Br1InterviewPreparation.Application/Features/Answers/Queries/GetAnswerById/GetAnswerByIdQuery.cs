using MediatR;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;

namespace Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerById;

public class GetAnswerByIdQuery : IRequest<AnswerDto>
{
    public required Guid Id { get; set; }
}
