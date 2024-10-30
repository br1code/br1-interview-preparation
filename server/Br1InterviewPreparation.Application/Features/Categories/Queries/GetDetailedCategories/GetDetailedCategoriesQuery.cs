using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;

namespace Br1InterviewPreparation.Application.Features.Categories.Queries.GetDetailedCategories;

public class GetDetailedCategoriesQuery : IRequest<IEnumerable<CategoryDetailsDto>>;