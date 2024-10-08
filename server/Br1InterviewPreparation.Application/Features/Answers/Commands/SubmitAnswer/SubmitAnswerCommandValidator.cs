using FluentValidation;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandValidator : AbstractValidator<SubmitAnswerCommand>
{
    public const string VIDEOFILENAME_EMPTY_ERROR_MESSAGE = "Video Filename is required.";
    public const string QUESTION_ID_EMPTY_ERROR_MESSAGE = "Question Id is required.";
    public const string QUESTION_NOT_FOUND_ERROR_MESSAGE = "Question does not exist.";

    private readonly IQuestionRepository _questionRepository;

    public SubmitAnswerCommandValidator(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;

        RuleFor(x => x.VideoFilename)
            .NotEmpty()
            .WithMessage(VIDEOFILENAME_EMPTY_ERROR_MESSAGE);

        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage(QUESTION_ID_EMPTY_ERROR_MESSAGE);

        RuleFor(x => x.QuestionId)
            .MustAsync(QuestionExists)
            .WithMessage(QUESTION_NOT_FOUND_ERROR_MESSAGE);
    }

    private async Task<bool> QuestionExists(Guid questionId, CancellationToken cancellationToken)
    {
        return await _questionRepository.QuestionExists(questionId, cancellationToken);
    }
}
