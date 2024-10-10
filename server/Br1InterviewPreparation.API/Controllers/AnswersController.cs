using MediatR;
using Microsoft.AspNetCore.Mvc;
using Br1InterviewPreparation.Application.Features.Answers.Commands.DeleteAnswer;
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
        /// Deletes an answer and its associated video file.
        /// </summary>
        /// <param name="id">The ID of the answer to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Answer deleted successfully.</response>
        /// <response code="404">Answer not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAnswer(Guid id)
        {
            var command = new DeleteAnswerCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Inserts a new answer for a question.
        /// </summary>
        /// <param name="questionId">The ID of the question.</param>
        /// <param name="videoFile">The Video file.</param>
        /// <returns>The ID of the created answer.</returns>
        /// <response code="201">Answer created successfully.</response>
        /// <response code="400">Validation error occurred.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequestSizeLimit(52428800)] // 50MB
        public async Task<IActionResult> SubmitAnswer([FromForm] Guid questionId, [FromForm] IFormFile videoFile)
        {
            var fileUploadDto = new FileUploadDto
            {
                FileName = videoFile.FileName,
                ContentType = videoFile.ContentType,
                Content = await GetFileBytesAsync(videoFile)
            };

            var command = new SubmitAnswerCommand
            {
                QuestionId = questionId,
                VideoFile = fileUploadDto
            };

            var answerId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetAnswerMetadata), new { id = answerId }, answerId);
        }

        private static async Task<byte[]> GetFileBytesAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
