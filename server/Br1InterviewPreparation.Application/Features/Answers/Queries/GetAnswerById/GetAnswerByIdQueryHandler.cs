using MediatR;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerById;

public class GetAnswerByIdQueryHandler(IAnswerRepository repository) : IRequestHandler<GetAnswerByIdQuery, AnswerDto>
{
    public async Task<AnswerDto> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
    {
        var answer = await repository.GetAnswerByIdAsync(request.Id, cancellationToken);

        if (answer is null)
        {
            throw new NotFoundException(nameof(Answer), request.Id);
        }

        return new AnswerDto
        {
            Id = answer.Id,
            QuestionId = answer.QuestionId,
            VideoFilename = answer.VideoFilename
        };
    }
}
