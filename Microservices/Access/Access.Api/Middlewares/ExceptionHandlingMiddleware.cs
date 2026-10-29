using Access.Api.Responses;
using Access.Application.Exceptions;

namespace Access.Api.Middlewares
{
    // El mediador valida cada petición antes de llegar al caso de uso, por fuera del BaseHandler.
    // Este middleware convierte esa excepción (y cualquier error no controlado) en una respuesta
    // con el mismo formato ApiResponse que usan los controladores, sin exponer el stack trace.
    public sealed class ExceptionHandlingMiddleware
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
            catch (CustomValidationException ex) when (!context.Response.HasStarted)
            {
                IReadOnlyList<ApiError> errors = ex.Errors.Select(error => new ApiError($"Validation.{error.PropertyName}",
                                                                                        error.ErrorMessage,
                                                                                        "validation"))
                                                          .ToList();

                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, errors, "Debe ajustar los errores de validación");
            }
            catch (Exception ex) when (!context.Response.HasStarted)
            {
                _logger.LogError(ex, "Error no controlado procesando {Method} {Path}", context.Request.Method, context.Request.Path);

                IReadOnlyList<ApiError> errors = [new ApiError("Unexpected", "Ocurrió un error inesperado.", "unexpected")];

                await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, errors, "Ha ocurrido un error");
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, int statusCode, IReadOnlyList<ApiError> errors, string message)
        {
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(errors, message));
        }
    }
}
