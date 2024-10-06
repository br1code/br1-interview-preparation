using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Features.Answers;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using MediatR;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionWithAnswersDto>
{
    private readonly IQuestionRepository _repository;

    public GetQuestionByIdQueryHandler(IQuestionRepository repository)
    {
        _repository = repository;
    }

    public async Task<QuestionWithAnswersDto> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _repository.GetQuestionByIdAsync(request.Id, cancellationToken);

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
            })
        };
    }
}
