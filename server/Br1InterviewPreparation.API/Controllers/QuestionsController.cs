using MediatR;
using Microsoft.AspNetCore.Mvc;
using Br1InterviewPreparation.Application.Features.Questions.Dtos;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestionById;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetQuestions;
using Br1InterviewPreparation.Application.Features.Questions.Queries.GetRandomQuestion;
using Br1InterviewPreparation.Application.Features.Questions.Commands.AddQuestion;
using Br1InterviewPreparation.Application.Features.Questions.Commands.UpdateQuestion;
using Br1InterviewPreparation.Application.Features.Questions.Commands.DeleteQuestion;

namespace Br1InterviewPreparation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Retrieves a list of questions. Optionally filters by category.
        /// </summary>
        /// <param name="categoryId">Optional category to filter questions by.</param>
        /// <returns>A list of questions.</returns>
        /// <response code="200">Returns the list of questions</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuestionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestions([FromQuery] Guid? categoryId = null)
        {
            var query = new GetQuestionsQuery { CategoryId = categoryId };
            var questions = await mediator.Send(query);
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
        [ProducesResponseType(typeof(QuestionWithAnswersDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            var query = new GetQuestionByIdQuery { Id = id };
            var question = await mediator.Send(query);
            return Ok(question);
        }

        /// <summary>
        /// Retrieves a random question. Optionally filters by category.
        /// </summary>
        /// <param name="categoryId">Optional category to filter questions by.</param>
        /// <returns>A random question.</returns>
        /// <response code="200">A question.</response>
        [HttpGet("random")]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRandomQuestion([FromQuery] Guid? categoryId = null)
        {
            var query = new GetRandomQuestionQuery { CategoryId = categoryId };
            var question = await mediator.Send(query);
            return Ok(question);
        }

        /// <summary>
        /// Inserts a new question.
        /// </summary>
        /// <param name="command">The question to create.</param>
        /// <returns>The ID of the created question.</returns>
        /// <response code="201">Question created successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestionCommand command)
        {
            var questionId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetQuestionById), new { id = questionId }, questionId);
        }

        /// <summary>
        /// Updates a question.
        /// </summary>
        /// <param name="id">The ID of the question to update.</param>
        /// <param name="command">The content of the question to update.</param>
        /// <returns>The updated question.</returns>
        /// <response code="200">Question updated successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        /// <response code="404">Question not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] UpdateQuestionCommand command)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid question ID.");
            }

            command.Id = id;

            var updatedQuestion = await mediator.Send(command);
            return Ok(updatedQuestion);
        }

        /// <summary>
        /// Deletes a question by ID.
        /// </summary>
        /// <param name="id">The ID of the question to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Question deleted successfully.</response>
        /// <response code="404">Question not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var command = new DeleteQuestionCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}
