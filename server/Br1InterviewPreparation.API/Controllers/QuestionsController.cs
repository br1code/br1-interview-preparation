using MediatR;
using Microsoft.AspNetCore.Mvc;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestionById;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetRandomQuestion;

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

        /// <summary>
        /// Retrieves a question by ID.
        /// </summary>
        /// <param name="id">The ID of the question.</param>
        /// <returns>The question with its answers.</returns>
        /// <response code="200">A question.</response>
        /// <response code="404">Question not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(QuestionWithAnswersDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            var query = new GetQuestionByIdQuery { Id = id };
            var question = await _mediator.Send(query);
            return Ok(question);
        }

        /// <summary>
        /// Retrieves a random question. Optionally filters by category.
        /// </summary>
        /// <param name="categoryId">Optional category to filter questions by.</param>
        /// <returns>A random question.</returns>
        /// <response code="200">A question.</response>
        [HttpGet("random")]
        [ProducesResponseType(typeof(QuestionDto), 200)]
        public async Task<IActionResult> GetRandomQuestion([FromQuery] Guid? categoryId = null)
        {
            var query = new GetRandomQuestionQuery { CategoryId = categoryId };
            var question = await _mediator.Send(query);
            return Ok(question);
        }
    }
}
