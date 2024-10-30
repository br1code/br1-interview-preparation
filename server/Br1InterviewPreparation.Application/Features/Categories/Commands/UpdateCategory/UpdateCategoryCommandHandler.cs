using MediatR;
using Br1InterviewPreparation.Application.Exceptions;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;
using Br1InterviewPreparation.Application.Interfaces;
using Br1InterviewPreparation.Domain.Entities;

namespace Br1InterviewPreparation.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetCategoryByIdAsync(request.Id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }
        
        category.Name = request.Name;
        
        await categoryRepository.UpdateCategoryAsync(category, cancellationToken);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}