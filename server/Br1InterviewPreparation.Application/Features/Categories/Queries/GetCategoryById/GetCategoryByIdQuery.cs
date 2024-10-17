using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;

namespace Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<CategoryDto>
{
    public required Guid Id { get; set; }
}
