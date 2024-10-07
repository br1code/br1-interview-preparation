using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;

namespace Br1InterviewPreparation.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommand : IRequest<QuestionDto>
{
    public Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public required string Content { get; set; }
    public string? Hint {  get; set; } 
}
