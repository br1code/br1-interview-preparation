using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Br1InterviewPreparation.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToArray();
            var result = JsonSerializer.Serialize(new { errors });
            await context.Response.WriteAsync(result);
        }
        // TODO: Handle other exceptions like not found ...
    }
}
