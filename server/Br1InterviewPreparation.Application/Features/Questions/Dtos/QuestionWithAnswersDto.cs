using Br1InterviewPreparation.Application.Features.Answers;

namespace Br1InterviewPreparation.Application.Features.Questions.Dtos;

public class QuestionWithAnswersDto
{
    public required Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public required string Content { get; set; }
    public string? Hint { get; set; }
    public IEnumerable<AnswerDto> Answers { get; set; } = [];
}


