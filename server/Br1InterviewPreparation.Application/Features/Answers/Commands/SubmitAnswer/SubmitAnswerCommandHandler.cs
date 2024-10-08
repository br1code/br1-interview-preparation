using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandHandler(IAnswerRepository answerRepository) : IRequestHandler<SubmitAnswerCommand, Guid>
{
    public async Task<Guid> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
    {
        // TODO: upload video file and store the filename
        var answer = new Answer
        {
            QuestionId = request.QuestionId,
            VideoFilename = request.VideoFilename,
        };

        await answerRepository.AddAnswerAsync(answer, cancellationToken);
        return answer.Id;
    }
}
