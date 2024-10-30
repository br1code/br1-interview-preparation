using FluentValidation;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandValidator : AbstractValidator<SubmitAnswerCommand>
{
    public const string EmptyVideoFileErrorMessage = "VideoFile is required.";
    public const string EmptyFileNameErrorMessage = "VideoFile file name is required.";
    public const string EmptyContentTypeErrorMessage = "VideoFile content type is required.";
    public const string InvalidContentTypeErrorMessage = "Invalid content type. Must be a video.";
    public const string EmptyContentErrorMessage = "VideoFile content is required.";
    public const string EmptyQuestionIdErrorMessage = "Question Id is required.";
    public const string QuestionNotFoundErrorMessage = "Question does not exist.";

    private readonly IQuestionRepository _questionRepository;

    public SubmitAnswerCommandValidator(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
        
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage(EmptyQuestionIdErrorMessage);

        RuleFor(x => x.QuestionId)
            .MustAsync(QuestionExists)
            .WithMessage(QuestionNotFoundErrorMessage);

        RuleFor(x => x.VideoFile)
            .NotNull()
            .WithMessage(EmptyVideoFileErrorMessage)
            .DependentRules(() =>
            {
                RuleFor(x => x.VideoFile.FileName)
                    .NotEmpty()
                    .WithMessage(EmptyFileNameErrorMessage);

                RuleFor(x => x.VideoFile.ContentType)
                    .NotEmpty()
                    .WithMessage(EmptyContentTypeErrorMessage)
                    .Must(contentType => !string.IsNullOrEmpty(contentType) && contentType.StartsWith("video/"))
                    .WithMessage(InvalidContentTypeErrorMessage);

                RuleFor(x => x.VideoFile.Content)
                    .NotEmpty()
                    .WithMessage(EmptyContentErrorMessage);
            });
    }

    private async Task<bool> QuestionExists(Guid questionId, CancellationToken cancellationToken)
    {
        return await _questionRepository.QuestionExists(questionId, cancellationToken);
    }
}
