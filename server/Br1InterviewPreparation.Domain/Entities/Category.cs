namespace Br1InterviewPreparation.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Question> Questions { get; set; } = [];
}
