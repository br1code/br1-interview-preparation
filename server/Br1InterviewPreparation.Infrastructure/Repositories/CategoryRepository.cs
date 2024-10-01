using Microsoft.EntityFrameworkCore;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Infrastructure.Data;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = _context.Categories.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
