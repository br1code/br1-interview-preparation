﻿using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Interfaces;

public interface IQuestionRepository
{
    Task<List<Question>> GetQuestionsWithAnswersAsync(Guid? categoryId = null, string? content = null,
        int pageNumber = 1, int? pageSize = null, CancellationToken cancellationToken = default);

    Task<Question?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Question?> GetQuestionWithAnswersByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Question?> GetRandomQuestionAsync(Guid? categoryId = null, CancellationToken cancellationToken = default);
    Task AddQuestionAsync(Question question, CancellationToken cancellationToken = default);
    Task UpdateQuestionAsync(Question question, CancellationToken cancellationToken = default);
    Task DeleteQuestionAsync(Question question, CancellationToken cancellationToken = default);
    Task<bool> QuestionExists(Guid id, CancellationToken cancellationToken = default);
}