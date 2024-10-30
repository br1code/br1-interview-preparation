using MediatR;

namespace Br1InterviewPreparation.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
}