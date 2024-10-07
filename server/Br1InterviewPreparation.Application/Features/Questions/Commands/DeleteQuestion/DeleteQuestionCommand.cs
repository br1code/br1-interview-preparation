using MediatR;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
