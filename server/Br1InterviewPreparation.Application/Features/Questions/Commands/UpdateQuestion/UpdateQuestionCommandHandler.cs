using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionDto>
{
    private readonly IQuestionRepository _questionRepository;

    public UpdateQuestionCommandHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<QuestionDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetQuestionByIdAsync(request.QuestionId, cancellationToken);

        if (question is null)
        {
            throw new NotFoundException(nameof(Question), request.QuestionId);
        }

        question.CategoryId = request.CategoryId;
        question.Content = request.Content;
        question.Hint = request.Hint;

        await _questionRepository.UpdateQuestionAsync(question, cancellationToken);

        return new QuestionDto
        {
            Id = question.Id,
            CategoryId = question.CategoryId,
            Content = question.Content,
            Hint = question.Hint,
        };
    }
}
