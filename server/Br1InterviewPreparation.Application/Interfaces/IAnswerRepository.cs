using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Interfaces;

public interface IAnswerRepository
{
    Task<Answer?> GetAnswerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAnswerAsync(Answer answer, CancellationToken cancellationToken = default);
    Task DeleteAnswerAsync(Answer answer, CancellationToken cancellationToken = default);
}
