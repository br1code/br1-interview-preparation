using Microsoft.EntityFrameworkCore;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Infrastructure.Data;

namespace Br1InterviewPreparation.Infrastructure.Repositories;

public class QuestionRepository(ApplicationDbContext context) : IQuestionRepository
{
    public Task<List<Question>> GetQuestionsWithAnswersAsync(Guid? categoryId = null, string? content = null,
        int pageNumber = 1, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        var query = context.Questions
            .Include(q => q.Answers)
            .AsNoTracking();

        if (categoryId != null && categoryId != Guid.Empty)
        {
            query = query.Where(c => c.CategoryId == categoryId);
        }

        if (!string.IsNullOrWhiteSpace(content))
        {
            query = query.Where(q => q.Content.ToLower().Contains(content.ToLower()));
        }

        if (pageSize.HasValue)
        {
            query = query
                .Skip((pageNumber - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return query.ToListAsync(cancellationToken);
    }


    public Task<Question?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Questions
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public Task<Question?> GetQuestionWithAnswersByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Questions
            .Include(q => q.Answers)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public Task<Question?> GetRandomQuestionAsync(Guid? categoryId = null,
        CancellationToken cancellationToken = default)
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