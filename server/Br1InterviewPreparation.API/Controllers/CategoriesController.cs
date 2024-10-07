using Microsoft.AspNetCore.Mvc;
using MediatR;
using Br1InterviewPreparation.Application.Features.Categories.Dtos;
using Br1InterviewPreparation.Application.Features.Categories.Queries.GetCategories;

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
    }
}
