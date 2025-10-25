using System.ComponentModel.DataAnnotations;
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
                _logger.LogError("Создание животного провалено. Validation failed ! Invalid input data! Error: {Error}", ex);
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest, "Invalid input data! The animal species field is filled in incorrectly");
            }

            catch (ValidationException ex)
            {
                _logger.LogError($"Животное с таким именем уже существует: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest, "Already exists with this name");
            }

            catch (SqlNullValueException ex)
            {
                _logger.LogError("Возврат всех животных пользователю провалено. Нет никаких животных в БД !");
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.NotFound, "There are no animals in the zoo");
            }

            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Животное с таким Id не было найдено: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.NotFound, "Animal Not Found");
            }

            catch (Exception ex)
            {
                _logger.LogCritical("Какая-то дыра/баг в приложении т.к. данное исключение не должно обрабатываться при нормальной работе !!!");
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError, "Something strange happened");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string exMessage, HttpStatusCode statusCode, string message)
        {
            _logger.LogError(exMessage);
            HttpResponse response = context.Response;
            response.StatusCode = (int)statusCode;

            ErrorResponceDto errorDto = new()
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
