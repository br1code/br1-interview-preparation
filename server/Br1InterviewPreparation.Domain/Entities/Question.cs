using Br1InterviewPreparation.Domain.Common;

namespace Br1InterviewPreparation.Domain.Entities;

public class Question : AuditableEntity
{
    public Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public required string Content { get; set; }
    public string? Hint { get; set; }
    public required Category Category { get; set; }
    public ICollection<Answer> Answers { get; set; } = [];
}
