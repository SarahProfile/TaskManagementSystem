using System.Net;
using System.Text.Json;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        var response = context.Response;
        response.ContentType = "application/json";

        var apiResponse = new ApiResponse<object>
        {
            Success = false,
            Data = null
        };

        switch (exception)
        {
            case NotFoundException notFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                apiResponse.Message = notFoundException.Message;
                _logger.LogWarning(notFoundException, "Not Found: {Message}", notFoundException.Message);
                break;

            case BadRequestException badRequestException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                apiResponse.Message = badRequestException.Message;
                _logger.LogWarning(badRequestException, "Bad Request: {Message}", badRequestException.Message);
                break;

            case UnauthorizedException unauthorizedException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                apiResponse.Message = unauthorizedException.Message;
                _logger.LogWarning(unauthorizedException, "Unauthorized: {Message}", unauthorizedException.Message);
                break;

            case FluentValidation.ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Errors.ToList();
                var errorMessages = errors.Select(e => e.ErrorMessage).ToList();
                apiResponse.Message = string.Join(". ", errorMessages);
                apiResponse.Data = new
                {
                    Errors = errors.GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                };
                _logger.LogWarning(validationException, "Validation Error: {Errors}", string.Join(", ", errorMessages));
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                apiResponse.Message = "An internal server error occurred. Please try again later.";
                _logger.LogError(exception, "Internal Server Error: {Message}", exception.Message);
                break;
        }

        var result = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }
}
