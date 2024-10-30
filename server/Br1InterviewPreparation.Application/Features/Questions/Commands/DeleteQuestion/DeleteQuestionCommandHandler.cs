using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.DeleteQuestion;

// TODO: add logging
public class DeleteQuestionCommandHandler(IQuestionRepository questionRepository, IVideoStorageService videoStorageService) : IRequestHandler<DeleteQuestionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetQuestionWithAnswersByIdAsync(request.Id, cancellationToken);

        if (question is null)
        {
            throw new NotFoundException(nameof(Question), request.Id);
        }

        foreach (var answer in question.Answers)
        {
            videoStorageService.DeleteVideoFile(answer.VideoFilename);
        }

        await questionRepository.DeleteQuestionAsync(question, cancellationToken);

        return Unit.Value;
    }
}
