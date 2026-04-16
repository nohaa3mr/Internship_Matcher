using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InternshipMatcher.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(IProblemDetailsService problemDetails) : IExceptionHandler
    {
        private readonly IProblemDetailsService problemDetails = problemDetails;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError,

            };
            return await problemDetails.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Detail = exception.Message,
                    Status = httpContext.Response.StatusCode,
                    Title = "An error occurred while processing your request.",
                    Extensions =
                {
                    ["traceId"] = httpContext.TraceIdentifier
                },
                    Instance = httpContext.Request.Path,
                    Type = exception.GetType().Name
                },
            });
        }
    }
}
