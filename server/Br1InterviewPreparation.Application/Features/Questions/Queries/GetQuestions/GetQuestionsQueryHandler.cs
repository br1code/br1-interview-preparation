﻿using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsQueryHandler(IQuestionRepository repository) : IRequestHandler<GetQuestionsQuery, IEnumerable<QuestionDto>>
{
    public async Task<IEnumerable<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await repository.GetQuestionsAsync(request.CategoryId, cancellationToken);

        return questions.Select(x => new QuestionDto 
        {
            Id = x.Id,
            CategoryId = x.CategoryId,
            Content = x.Content,
            Hint = x.Hint,
        });
    }
}
