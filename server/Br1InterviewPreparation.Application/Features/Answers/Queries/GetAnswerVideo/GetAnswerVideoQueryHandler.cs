using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerVideo;

public class GetAnswerVideoQueryHandler(IAnswerRepository answerRepository) : IRequestHandler<GetAnswerVideoQuery, AnswerVideoDto>
{
    private const string VIDEO_STORE_PATH = "/videos/";
    private const string VIDEO_CONTENT_TYPE = "video/webm";

    public async Task<AnswerVideoDto> Handle(GetAnswerVideoQuery request, CancellationToken cancellationToken)
    {
        var answer = await answerRepository.GetAnswerByIdAsync(request.Id, cancellationToken);

        if (answer is null)
        {
            throw new NotFoundException(nameof(answer), request.Id);
        }

        var filePath = Path.Combine(VIDEO_STORE_PATH, answer.VideoFilename);
        return new AnswerVideoDto
        {
            FilePath = filePath,
            ContentType = VIDEO_CONTENT_TYPE,
        };
    }
}
