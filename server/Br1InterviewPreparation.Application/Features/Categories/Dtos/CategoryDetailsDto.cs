namespace Br1InterviewPreparation.Application.Features.Categories.Dtos;

public class CategoryDetailsDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required int QuestionsCount { get; set; }
}