using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<Category>> GetDetailedCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CategoryExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default);
    Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
}
