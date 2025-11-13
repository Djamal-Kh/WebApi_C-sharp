using Npgsql;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Net;
using ZooApi.DTO;

namespace ZooApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }

            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError($"Значение вне диапозона допустимых значений: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError, "Argument out of Range");
            }

            catch (ArgumentException ex)
            {
                _logger.LogError("Перехвачено исключение ArgumentException: {exception}", ex);
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest, "Invalid input data! The animal species field is filled in incorrectly");
            }

            catch (NpgsqlException ex)
            {
                _logger.LogError("Что-то случилось с PostgreSql", ex);
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.ServiceUnavailable, "Invalid input data! The animal species field is filled in incorrectly");
            }

            catch (Exception ex)
            {
                _logger.LogCritical("!!! UNHANDLED EXCEPTION !!!");
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError, "Something strange happened");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string exMessage, HttpStatusCode statusCode, string message)
        {
            _logger.LogError(exMessage);
            HttpResponse response = context.Response;
            response.StatusCode = (int)statusCode;

            ErrorDetailsDto errorDto = new()
            {
                Message = message,
                StatusCode = (int)statusCode
            };
            await response.WriteAsJsonAsync(errorDto);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
