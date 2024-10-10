using FluentValidation;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;

public class SubmitAnswerCommandValidator : AbstractValidator<SubmitAnswerCommand>
{
    public const string EMPTY_VIDEO_FILE_ERROR_MESSAGE = "VideoFile is required.";
    public const string EMPTY_FILE_NAME_ERROR_MESSAGE = "VideoFile file name is required.";
    public const string EMPTY_CONTENT_TYPE_ERROR_MESSAGE = "VideoFile content type is required.";
    public const string INVALID_CONTENT_TYPE_ERROR_MESSAGE = "Invalid content type. Must be a video.";
    public const string EMPTY_CONTENT_ERROR_MESSAGE = "VideoFile content is required.";
    public const string EMPTY_QUESTION_ID_ERROR_MESSAGE = "Question Id is required.";
    public const string QUESTION_NOT_FOUND_ERROR_MESSAGE = "Question does not exist.";

    private readonly IQuestionRepository _questionRepository;

    public SubmitAnswerCommandValidator(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;

        RuleFor(x => x.VideoFile)
            .NotNull()
            .WithMessage(EMPTY_VIDEO_FILE_ERROR_MESSAGE);

        RuleFor(x => x.VideoFile.FileName)
            .NotEmpty()
            .WithMessage(EMPTY_FILE_NAME_ERROR_MESSAGE);

        RuleFor(x => x.VideoFile.ContentType)
            .NotEmpty()
            .WithMessage(EMPTY_CONTENT_TYPE_ERROR_MESSAGE)
            .Must(contentType => !string.IsNullOrEmpty(contentType) && contentType.StartsWith("video/"))
            .WithMessage(INVALID_CONTENT_TYPE_ERROR_MESSAGE);

        RuleFor(x => x.VideoFile.Content)
            .NotEmpty()
            .WithMessage(EMPTY_CONTENT_ERROR_MESSAGE);

        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage(EMPTY_QUESTION_ID_ERROR_MESSAGE);

        RuleFor(x => x.QuestionId)
            .MustAsync(QuestionExists)
            .WithMessage(QUESTION_NOT_FOUND_ERROR_MESSAGE);
    }

    private async Task<bool> QuestionExists(Guid questionId, CancellationToken cancellationToken)
    {
        return await _questionRepository.QuestionExists(questionId, cancellationToken);
    }
}
