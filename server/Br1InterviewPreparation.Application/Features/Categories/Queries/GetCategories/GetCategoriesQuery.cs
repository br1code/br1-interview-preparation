using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;

namespace Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
