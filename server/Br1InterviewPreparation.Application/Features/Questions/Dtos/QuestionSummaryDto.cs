namespace Br1InterviewPreparation.Application.Features.Questions.Dtos;

public class QuestionSummaryDto
{
    public required Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public required string Content { get; set; }
    public string? Hint { get; set; }
    public required int AnswersCount { get; set; }
}