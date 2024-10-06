using Microsoft.EntityFrameworkCore;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Infrastructure.Data;

namespace Br1InterviewPreparation.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public QuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Question>> GetQuestionsAsync(Guid? categoryId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Questions.AsNoTracking();

        if (categoryId != null && categoryId != Guid.Empty)
        {
            query = query.Where(c => c.CategoryId == categoryId);
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<Question?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Questions
            .AsNoTracking()
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public Task<Question?> GetRandomQuestionAsync(Guid? categoryId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Questions
            .AsNoTracking();

        if (categoryId != null && categoryId != Guid.Empty)
        {
            query = query.Where(c => c.CategoryId == categoryId);
        }

        return query
            .OrderBy(k => EF.Functions.Random())
            .FirstOrDefaultAsync(cancellationToken);
    }
}
