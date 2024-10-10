using MediatR;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
}
