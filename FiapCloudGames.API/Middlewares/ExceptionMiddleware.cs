using FluentValidation;
using System.Net;
using System.Text.Json;

namespace FiapCloudGames.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            await HandleValidationException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericException(context, ex);
        }
    }

    private async Task HandleValidationException(HttpContext context, ValidationException ex)
    {
        var errorMessages = ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();

        // Log mais rico e útil
        _logger.LogWarning(ex,
            "Erro de validação na requisição. " +
            "Path: {RequestPath} | Method: {Method} | Erros: {ValidationErrors}",
            context.Request.Path,
            context.Request.Method,
            string.Join(" | ", errorMessages));

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var response = new
        {
            title = "Erro de validação",
            status = (int)HttpStatusCode.BadRequest,
            errors,
            traceId = context.TraceIdentifier
        };

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private async Task HandleGenericException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex,
            "Erro não tratado ocorrido na aplicação. " +
            "Path: {RequestPath} | Method: {Method}",
            context.Request.Path,
            context.Request.Method);

        var response = new
        {
            title = "Erro interno do servidor",
            status = (int)HttpStatusCode.InternalServerError,
            detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
            traceId = context.TraceIdentifier
        };

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}