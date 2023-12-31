using System.Net;
using System.Text.Json;
using SampleApi.Common.Core.BusinessRules;
using Microsoft.AspNetCore.Http;

namespace SampleApi.Common.Api.ErrorHandling;

public sealed class ExceptionMiddleware
{
    private const string ContentType = "application/json";
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = ContentType;

        int statusCode;
        string message;

        switch (exception)
        {
            case BusinessRuleValidationException businessRuleValidationException:
                statusCode = (int)HttpStatusCode.Conflict;
                message = businessRuleValidationException.Message;
                break;
            case ResourceNotFoundException resourceNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = resourceNotFoundException.Message;
                break;
            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = exception.Message;
                break;
        }

        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new ExceptionResponseMessage(statusCode, message));

        return context.Response.WriteAsync(result);
    }
}

public record ExceptionResponseMessage(int StatusCode, string Message);
