using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;

namespace Br1InterviewPreparation.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryDto>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}