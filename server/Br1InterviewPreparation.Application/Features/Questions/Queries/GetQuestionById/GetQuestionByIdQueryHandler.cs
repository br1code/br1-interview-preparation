using MediatR;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQueryHandler(IQuestionRepository repository) : IRequestHandler<GetQuestionByIdQuery, QuestionWithAnswersDto>
{
    public async Task<QuestionWithAnswersDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await repository.GetQuestionByIdAsync(request.Id, cancellationToken);

        if (question is null)
        {
            throw new NotFoundException(nameof(Question), request.Id);
        }

        return new QuestionWithAnswersDto
        {
            Id = question.Id,
            CategoryId = question.CategoryId,
            Content = question.Content,
            Hint = question.Hint,
            Answers = question.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                QuestionId = a.Id,
                VideoFilename = a.VideoFilename,
                CreatedAt = a.CreatedAt
            })
        };
    }
}
