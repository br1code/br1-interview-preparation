using Microsoft.AspNetCore.Mvc;
using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategories;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategoryById;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetDetailedCategories;
using Br1InterviewPreparation.Application.Features.Categories.Commands.AddCategory;
using Br1InterviewPreparation.Application.Features.Categories.Commands.DeleteCategory;
using Br1InterviewPreparation.Application.Features.Categories.Commands.UpdateCategory;

namespace Br1InterviewPreparation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves a list of categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        /// <response code="200">Returns the list of categories</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var query = new GetCategoriesQuery();
            var categories = await mediator.Send(query);
            return Ok(categories);
        }

        /// <summary>
        /// Retrieves a list of categories, including some details.
        /// </summary>
        /// <returns>A list of categories.</returns>
        /// <response code="200">Returns the list of categories</response>
        [HttpGet("detailed")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDetailsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailedCategories()
        {
            var query = new GetDetailedCategoriesQuery();
            var categories = await mediator.Send(query);
            return Ok(categories);
        }

        /// <summary>
        /// Retrieves a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category.</returns>
        /// <response code="200">A category.</response>
        /// <response code="404">Category not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var category = await mediator.Send(query);
            return Ok(category);
        }

        /// <summary>
        /// Inserts a new category.
        /// </summary>
        /// <param name="command">The category to create.</param>
        /// <returns>The ID of the created category.</returns>
        /// <response code="201">Category created successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand command)
        {
            var categoryId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryId }, categoryId);
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="command">The content of the category to update.</param>
        /// <returns>The updated category.</returns>
        /// <response code="200">Category updated successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        /// <response code="404">Category not found.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommand command)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid category ID.");
            }

            command.Id = id;

            var updatedCategory = await mediator.Send(command);
            return Ok(updatedCategory);
        }
        
        /// <summary>
        /// Deletes a category by ID, including its related questions and answers.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Category deleted successfully.</response>
        /// <response code="404">Category not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}
