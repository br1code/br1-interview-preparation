using MediatR;

namespace Br1InterviewPreparation.Application.Features.Categories.Commands.AddCategory;

public class AddCategoryCommand : IRequest<Guid>
{
    public required string Name { get; set; }
}