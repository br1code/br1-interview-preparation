using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler(IQuestionRepository questionRepository) : IRequestHandler<UpdateQuestionCommand, QuestionDto>
{
    public async Task<QuestionDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetQuestionByIdAsync(request.Id, cancellationToken);

        if (question is null)
        {
            throw new NotFoundException(nameof(Question), request.Id);
        }

        question.CategoryId = request.CategoryId;
        question.Content = request.Content;
        question.Hint = request.Hint;

        await questionRepository.UpdateQuestionAsync(question, cancellationToken);

        return new QuestionDto
        {
            Id = question.Id,
            CategoryId = question.CategoryId,
            Content = question.Content,
            Hint = question.Hint,
        };
    }
}
