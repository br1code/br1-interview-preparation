using System.Net;
using System.Text.Json;
using FluentValidation;
using Br1InterviewPreparation.Application.Exceptions;

namespace Br1InterviewPreparation.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation exception occurred: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Not found exception occurred: {Message}", ex.Message);
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleGenericExceptionAsync(context);
        }

    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var errors = ex.Errors.Select(e => e.ErrorMessage).ToArray();
        var result = JsonSerializer.Serialize(new { errors });
        await context.Response.WriteAsync(result);
    }

    private async Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { error = ex.Message });
        await context.Response.WriteAsync(result);
    }

    private async Task HandleGenericExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
        await context.Response.WriteAsync(result);
    }
}
