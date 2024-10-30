using Microsoft.EntityFrameworkCore;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Infrastructure.Data;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Infrastructure.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var query = context.Categories.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<List<Category>> GetDetailedCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var query = context.Categories
            .Include(c => c.Questions)
            .AsNoTracking();
        
        return query.ToListAsync(cancellationToken);
    }

    public Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<bool> CategoryExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Id == id, cancellationToken);
    }
}
