using Br1InterviewPreparation.Application.Features.Categories.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using MediatR;

namespace Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _repository;

    public GetCategoriesQueryHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(cancellationToken);

        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
        });
    }
}
