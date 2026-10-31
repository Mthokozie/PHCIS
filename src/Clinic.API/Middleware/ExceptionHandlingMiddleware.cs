namespace Clinic.API.Middleware;

using System.Net;
using System.Text.Json;
using Clinic.Application.Common.Exceptions;
using Clinic.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Global exception handling middleware that maps domain and application exceptions
/// to standardized RFC 7807 <see cref="ProblemDetails"/> HTTP responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>Initialises a new instance of <see cref="ExceptionHandlingMiddleware"/>.</summary>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>Invokes the middleware pipeline and catches unhandled exceptions.</summary>
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
        _logger.LogError(exception, "An exception occurred while processing {Method} {Path}",
            context.Request.Method, context.Request.Path);

        var (statusCode, title, errors) = exception switch
        {
            NotFoundException => ((int)HttpStatusCode.NotFound, "Resource Not Found", null),
            ValidationException validationEx => ((int)HttpStatusCode.BadRequest, "Validation Error", validationEx.Errors),
            ConflictException => ((int)HttpStatusCode.Conflict, "Conflict", null),
            DomainException => ((int)HttpStatusCode.BadRequest, "Domain Rule Violation", null),
            _ => ((int)HttpStatusCode.InternalServerError, "Server Error", null)
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == 500 ? "An unexpected error occurred. Please try again later." : exception.Message,
            Instance = context.Request.Path
        };

        if (errors is not null)
        {
            problemDetails.Extensions["errors"] = errors;
        }

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }
}
