using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Categories.Queries.GetDetailedCategories;

public class GetDetailedCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetDetailedCategoriesQuery, IEnumerable<CategoryDetailsDto>>
{
    public async Task<IEnumerable<CategoryDetailsDto>> Handle(GetDetailedCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await repository.GetDetailedCategoriesAsync(cancellationToken);

        return categories.Select(category => new CategoryDetailsDto
        {
            Id = category.Id,
            Name = category.Name,
            QuestionsCount = category.Questions.Count,
        });
    }
}