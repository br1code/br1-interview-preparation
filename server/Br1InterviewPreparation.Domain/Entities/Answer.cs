using Br1InterviewPreparation.Domain.Common;

namespace Br1InterviewPreparation.Domain.Entities;

public class Answer : AuditableEntity
{
    public Guid Id { get; set; }
    public required Guid QuestionId { get; set; }
    public required string VideoFilename { get; set; }
    public required Question Question { get; set; }
}
