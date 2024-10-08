using MediatR;
using Microsoft.AspNetCore.Mvc;
using Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;
using Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerById;

namespace Br1InterviewPreparation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves an answer by ID.
        /// </summary>
        /// <param name="id">The ID of the answer.</param>
        /// <returns>The answer.</returns>
        /// <response code="200">An answer.</response>
        /// <response code="404">Answer not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerById(Guid id)
        {
            var query = new GetAnswerByIdQuery { Id = id };
            var answer = await mediator.Send(query);
            return Ok(answer);
        }

        /// <summary>
        /// Inserts a new answer for a question.
        /// </summary>
        /// <param name="command">The answer to create.</param>
        /// <returns>The ID of the created answer.</returns>
        /// <response code="201">Answer created successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerCommand command)
        {
            var answerId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetAnswerById), new { id = answerId }, answerId);
        }
    }
}
