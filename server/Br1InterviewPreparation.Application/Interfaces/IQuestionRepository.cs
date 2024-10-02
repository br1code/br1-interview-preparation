﻿using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Interfaces;

public interface IQuestionRepository
{
    Task<List<Question>> GetQuestionsAsync(Guid? categoryId = null, CancellationToken cancellationToken = default);
}
