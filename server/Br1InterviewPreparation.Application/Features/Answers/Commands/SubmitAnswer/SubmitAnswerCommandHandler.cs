using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandHandler(IAnswerRepository answerRepository, IVideoStorageService videoStorageService) : IRequestHandler<SubmitAnswerCommand, Guid>
{
    public async Task<Guid> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
    {
        var videoFileName = await videoStorageService.SaveVideoFileAsync(
            request.VideoFile.FileName, request.VideoFile.ContentType, request.VideoFile.Content, cancellationToken);

        var answer = new Answer
        {
            QuestionId = request.QuestionId,
            VideoFilename = videoFileName
        };

        await answerRepository.AddAnswerAsync(answer, cancellationToken);
        return answer.Id;
    }
}
