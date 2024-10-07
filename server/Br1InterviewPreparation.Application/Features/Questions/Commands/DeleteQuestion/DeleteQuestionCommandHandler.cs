using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler(IQuestionRepository questionRepository) : IRequestHandler<DeleteQuestionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetQuestionByIdAsync(request.Id, cancellationToken);

        if (question is null)
        {
            throw new NotFoundException(nameof(question), request.Id);
        }

        await questionRepository.DeleteQuestionAsync(question, cancellationToken);

        // TODO: Delete video files of related Answers when implemented.
        // (we already delete Answer records from DB, we configured the model using Cascade delete behavior)

        return Unit.Value;
    }
}
