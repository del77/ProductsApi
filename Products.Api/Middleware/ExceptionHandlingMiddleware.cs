using System.Net;
using System.Text.Json;
using Products.Domain.Exceptions;

namespace Products.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

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
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ProductsException productsException => new ErrorResponse
            {
                ErrorCode = productsException.ErrorCode,
                Message = productsException.Message,
                StatusCode = (int)HttpStatusCode.BadRequest
            },
            _ => new ErrorResponse
            {
                ErrorCode = "INTERNAL_ERROR",
                Message = "Wystąpił nieoczekiwany błąd podczas przetwarzania żądania.",
                StatusCode = (int)HttpStatusCode.InternalServerError
            }
        };

        if (exception is ProductsException)
        {
            _logger.LogWarning(exception, "Business exception occurred: {ErrorCode} - {Message}", 
                ((ProductsException)exception).ErrorCode, exception.Message);
        }
        else
        {
            _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
        }

        context.Response.StatusCode = response.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(new
        {
            errorCode = response.ErrorCode,
            message = response.Message
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private class ErrorResponse
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
