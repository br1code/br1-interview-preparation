namespace Br1InterviewPreparation.Application.Features.Answers.Dtos;

public class AnswerDto
{
    public required Guid Id { get; set; }
    public required Guid QuestionId { get; set; }
    public required string VideoFilename { get; set; }
    public required DateTime CreatedAt { get; set; }
}
