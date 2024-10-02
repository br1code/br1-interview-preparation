using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Br1InterviewPreparation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of questions. Optionally filters by category.
        /// </summary>
        /// <param name="categoryId">Optional category to filter questions by.</param>
        /// <returns>A list of questions.</returns>
        /// <response code="200">Returns the list of questions</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuestionDto>), 200)]
        public async Task<IActionResult> GetQuestions([FromQuery] Guid? categoryId = null)
        {
            var query = new GetQuestionsQuery { CategoryId = categoryId };
            var questions = await _mediator.Send(query);
            return Ok(questions);
        }
    }
}
