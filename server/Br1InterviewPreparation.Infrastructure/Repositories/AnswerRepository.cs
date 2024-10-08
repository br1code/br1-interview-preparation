using Microsoft.EntityFrameworkCore;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;
using Br1InterviewPreparation.Infrastructure.Data;

namespace Br1InterviewPreparation.Infrastructure.Repositories;

public class AnswerRepository(ApplicationDbContext context) : IAnswerRepository
{
    public Task<Answer?> GetAnswerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Answers
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public Task AddAnswerAsync(Answer answer, CancellationToken cancellationToken = default)
    {
        context.Answers.Add(answer);
        return context.SaveChangesAsync(cancellationToken);
    }
}
