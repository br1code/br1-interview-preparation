using MediatR;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommand : IRequest<Guid>
{
    public required Guid QuestionId { get; set; }
    public required string VideoFilename { get; set; }
}
