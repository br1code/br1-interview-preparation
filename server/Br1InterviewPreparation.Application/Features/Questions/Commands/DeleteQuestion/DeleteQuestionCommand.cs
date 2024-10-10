using MediatR;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
}
