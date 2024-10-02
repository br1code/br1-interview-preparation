using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
