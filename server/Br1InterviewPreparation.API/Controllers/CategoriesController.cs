using Microsoft.AspNetCore.Mvc;
using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategories;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategoryById;

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
        /// Retrieves a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category.</returns>
        /// <response code="200">A category.</response>
        /// <response code="404">Category not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var query = new GetCategoryByIdQuery { Id = id };
            var category = await mediator.Send(query);
            return Ok(category);
        }
    }
}
