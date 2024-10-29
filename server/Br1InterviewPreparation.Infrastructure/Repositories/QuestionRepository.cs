using Microsoft.EntityFrameworkCore;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Infrastructure.Data;

namespace Br1InterviewPreparation.Infrastructure.Repositories;

public class QuestionRepository(ApplicationDbContext context) : IQuestionRepository
{
    public Task<List<Question>> GetQuestionsAsync(Guid? categoryId = null, CancellationToken cancellationToken = default)
    {
        var query = context.Questions
            .Include(q => q.Answers)
            .AsNoTracking();

        if (categoryId != null && categoryId != Guid.Empty)
        {
            query = query.Where(c => c.CategoryId == categoryId);
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<Question?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Questions
            .Include(q => q.Answers)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public Task<Question?> GetRandomQuestionAsync(Guid? categoryId = null, CancellationToken cancellationToken = default)
    {
        var query = context.Questions.AsNoTracking();

        if (categoryId != null && categoryId != Guid.Empty)
        {
            query = query.Where(c => c.CategoryId == categoryId);
        }

        return query
            .OrderBy(k => EF.Functions.Random())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task AddQuestionAsync(Question question, CancellationToken cancellationToken = default)
    {
        context.Questions.Add(question);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateQuestionAsync(Question question, CancellationToken cancellationToken)
    {
        context.Questions.Update(question);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteQuestionAsync(Question question, CancellationToken cancellationToken = default)
    {
        context.Questions.Remove(question);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> QuestionExists(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Questions
            .AsNoTracking()
            .AnyAsync(q => q.Id == id, cancellationToken);
    }
}
