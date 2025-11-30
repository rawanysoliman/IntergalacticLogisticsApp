using IntergalacticLogistics.Api.Logging;

namespace IntergalacticLogistics.Api.Logging;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next,ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }



    public async Task InvokeAsync(HttpContext context)
    {

        using (_logger.BeginRequestScope(context))
        {
            _logger.LogInformation( "Incoming request: {Method} {Path}",context.Request.Method, context.Request.Path);

            await _next(context);

            _logger.LogInformation("Completed request: {Method} {Path} with status {StatusCode}", context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode);
        }
    }
}

