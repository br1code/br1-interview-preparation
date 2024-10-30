using MediatR;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Categories.Commands.DeleteCategory;

// TODO: add logging
public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IVideoStorageService videoStorageService) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetCategoryWithQuestionsByIdAsync(request.Id, cancellationToken);
        
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }
        
        foreach (var question in category.Questions)
        {
            foreach (var answer in question.Answers)
            {
                videoStorageService.DeleteVideoFile(answer.VideoFilename);
            }
        }
        
        await categoryRepository.DeleteCategoryAsync(category, cancellationToken);

        return Unit.Value;
    }
}