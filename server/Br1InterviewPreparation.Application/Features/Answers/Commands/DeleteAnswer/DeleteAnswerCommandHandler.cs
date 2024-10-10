using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandHandler(IAnswerRepository answerRepository, IVideoStorageService videoStorageService) : IRequestHandler<DeleteAnswerCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await answerRepository.GetAnswerByIdAsync(request.Id, cancellationToken);

        if (answer is null)
        {
            throw new NotFoundException(nameof(Answer), request.Id);
        }

        await answerRepository.DeleteAnswerAsync(answer, cancellationToken);

        videoStorageService.DeleteVideoFile(answer.VideoFilename);

        return Unit.Value;
    }
}
