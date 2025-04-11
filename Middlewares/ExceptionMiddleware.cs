using System.Net;
using System.Text.Json;
using ExamApi.Exceptions;

namespace ExamApi.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une exception a été interceptée.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Une erreur interne est survenue.";
            string? detail = null;

            if (ex is AppException appEx)
            {
                statusCode = appEx.StatusCode;
                title = appEx.Message;
            }
            else
            {
                detail = ex.Message;
            }

            var problemDetails = new
            {
                type = "https://tools.ietf.org/html/rfc7807",
                title,
                status = statusCode,
                detail,
                instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
