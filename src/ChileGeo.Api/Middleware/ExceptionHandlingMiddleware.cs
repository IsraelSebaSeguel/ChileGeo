using System.Net;
using System.Text.Json;
using ChileGeo.Domain.Exceptions;

namespace ChileGeo.Api.Middleware;

/// <summary>Middleware pattern: single place that catches unhandled exceptions, logs them and
/// translates them into consistent JSON error responses instead of leaking stack traces.</summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, "Recurso no encontrado"),
            BusinessValidationException => (HttpStatusCode.BadRequest, "Solicitud inválida"),
            _ => (HttpStatusCode.InternalServerError, "Error interno del servidor")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Error no controlado procesando {Method} {Path}", context.Request.Method, context.Request.Path);
        }
        else
        {
            _logger.LogWarning(exception, "{Title} procesando {Method} {Path}", title, context.Request.Method, context.Request.Path);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = new
        {
            status = (int)statusCode,
            title,
            detail = exception.Message,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
