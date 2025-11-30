using Microsoft.AspNetCore.Http;

namespace IntergalacticLogistics.Api.Logging;

public static class LoggingExtensions
{
    public static IDisposable BeginRequestScope(this ILogger logger, HttpContext context)
    {

        var requestId = context.TraceIdentifier;
        return logger.BeginScope(new Dictionary<string, object?>
        {
            ["RequestId"] = requestId,
            ["Path"] = context.Request.Path.Value,
            ["Method"] = context.Request.Method
        });
    }

}


