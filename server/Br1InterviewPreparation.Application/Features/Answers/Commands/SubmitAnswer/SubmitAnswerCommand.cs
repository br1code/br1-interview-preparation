using MediatR;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommand : IRequest<Guid>
{
    public required Guid QuestionId { get; set; }
    public required FileUploadDto VideoFile { get; set; }
}
