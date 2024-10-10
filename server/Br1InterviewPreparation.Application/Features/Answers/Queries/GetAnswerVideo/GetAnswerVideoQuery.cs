using MediatR;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;

namespace Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerVideo;

public class GetAnswerVideoQuery : IRequest<AnswerVideoDto>
{
    public required Guid Id { get; set; }
}