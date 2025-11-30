using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticLogistics.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception ex)
        {
            var requestId = context.TraceIdentifier;
            using (_logger.BeginScope(new { RequestId = requestId }))
            {
                _logger.LogError(ex, "Unhandled exception occurred for request {Path}", context.Request.Path);
            }


            // RFC 7807 ProblemDetails with proper type URIs
            var problemDetails = ex switch
            {
                ArgumentNullException argEx => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807#section-3.1",
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = argEx.Message,
                    Instance = context.Request.Path
                },

                ArgumentOutOfRangeException argEx => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807#section-3.1",
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = argEx.Message,
                    Instance = context.Request.Path
                },

                ArgumentException argEx => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807#section-3.1",
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = argEx.Message,
                    Instance = context.Request.Path
                },

                KeyNotFoundException keyEx => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807#section-3.1",
                    Title = "Not Found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = keyEx.Message,
                    Instance = context.Request.Path
                },

                InvalidOperationException opEx => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807#section-3.1",
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = opEx.Message,
                    Instance = context.Request.Path
                },

                _ => new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7807#section-3.1",
                    Title = "Internal Server Error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "An unexpected error occurred while processing your request.",
                    Instance = context.Request.Path
                }
            };

            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }

}

