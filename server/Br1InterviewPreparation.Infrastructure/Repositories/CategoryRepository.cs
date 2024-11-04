using Microsoft.EntityFrameworkCore;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Infrastructure.Data;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Infrastructure.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var query = context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name);

        return query.ToListAsync(cancellationToken);
    }


    public Task<List<Category>> GetDetailedCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var query = context.Categories
            .Include(c => c.Questions)
            .AsNoTracking()
            .OrderBy(c => c.Name);

        return query.ToListAsync(cancellationToken);
    }

    public Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Category?> GetCategoryWithQuestionsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Categories
            .Include(c => c.Questions)
            .ThenInclude(q => q.Answers)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<bool> CategoryExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Categories
            .AsNoTracking()
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    public Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        context.Categories.Add(category);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        context.Categories.Update(category);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        context.Categories.Remove(category);
        return context.SaveChangesAsync(cancellationToken);
    }
}