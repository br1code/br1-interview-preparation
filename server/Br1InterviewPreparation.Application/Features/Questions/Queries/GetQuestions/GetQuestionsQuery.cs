using MediatR;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;

namespace Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsQuery : IRequest<IEnumerable<QuestionSummaryDto>>
{
    public Guid? CategoryId { get; set; }
    public string? Content { get; set; }
    public int PageNumber { get; set; } = 1;
    public int? PageSize { get; set; }
}
