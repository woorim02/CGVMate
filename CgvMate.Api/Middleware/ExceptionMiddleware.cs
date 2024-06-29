using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;

namespace CgvMate.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the exception middleware will not be executed.");
                throw;
            }
            _logger.LogWarning($"{ex}");

            ex = GetActualException(ex);

            context.Response.StatusCode = GetStatusCode(ex);
            context.Response.ContentType = MediaTypeNames.Text.Plain;

            var errorContent = _hostEnvironment.IsDevelopment() ? ex.Message : "예기치 못한 오류";
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "An unexpected error occurred!",
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        return context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static Exception GetActualException(Exception ex)
    {
        if (ex is AggregateException agg)
        {
            var inner = agg.InnerException;
            if (inner is not null)
            {
                return GetActualException(inner);
            }

            var inners = agg.InnerExceptions;
            if (inners.Count > 0)
            {
                return GetActualException(inners[0]);
            }
        }

        return ex;
    }

    private static int GetStatusCode(Exception ex)
    {
        return ex switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}