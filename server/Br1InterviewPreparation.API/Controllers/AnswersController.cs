using MediatR;
using Microsoft.AspNetCore.Mvc;
using Br1InterviewPreparation.Application.Features.Answers.Commands.SubmitAnswer;
using Br1InterviewPreparation.Application.Features.Answers.Dtos;
using Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerById;
using Br1InterviewPreparation.Application.Features.Answers.Queries.GetAnswerVideo;

namespace Br1InterviewPreparation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Stream the video associated with the specific answer ID.
        /// </summary>
        /// <param name="id">The ID of the answer.</param>
        /// <returns>The answer video.</returns>
        /// <response code="200">The answer video</response>
        /// <response code="404">Answer not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PhysicalFileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAnswerVideo(Guid id)
        {
            var query = new GetAnswerVideoQuery { Id = id };
            var result = await mediator.Send(query);
            return PhysicalFile(result.FilePath, result.ContentType, enableRangeProcessing: true);
        }


        /// <summary>
        /// Retrieve metadata for a specific answer.
        /// </summary>
        /// <param name="id">The ID of the answer.</param>
        /// <returns>The answer metadata.</returns>
        /// <response code="200">The answer metadata..</response>
        /// <response code="404">Answer not found.</response>
        [HttpGet("{id}/metadata")]
        [ProducesResponseType(typeof(AnswerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAnswerMetadata(Guid id)
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
            return CreatedAtAction(nameof(GetAnswerMetadata), new { id = answerId }, answerId);
        }
    }
}
