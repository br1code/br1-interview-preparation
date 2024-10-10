using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerVideo;

public class GetAnswerVideoQueryHandler(IAnswerRepository answerRepository, IVideoStorageService videoStorageService) : IRequestHandler<GetAnswerVideoQuery, AnswerVideoDto>
{
    public async Task<AnswerVideoDto> Handle(GetAnswerVideoQuery request, CancellationToken cancellationToken)
    {
        var answer = await answerRepository.GetAnswerByIdAsync(request.Id, cancellationToken);

        if (answer is null)
        {
            throw new NotFoundException(nameof(answer), request.Id);
        }

        var filePath = videoStorageService.GetVideoFilePath(answer.VideoFilename);
        var contentType = videoStorageService.GetContentType(answer.VideoFilename);

        return new AnswerVideoDto
        {
            FilePath = filePath,
            ContentType = contentType,
        };
    }
}
