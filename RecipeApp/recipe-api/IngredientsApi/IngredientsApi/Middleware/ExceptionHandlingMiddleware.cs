namespace IngredientsApi.Middleware
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
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorDetails = exception switch
            {
                KeyNotFoundException => new { StatusCode = StatusCodes.Status404NotFound, Message = "Resource not found." },
                ArgumentException => new { StatusCode = StatusCodes.Status400BadRequest, Message = "Invalid input provided." },
                OperationCanceledException => new { StatusCode = 499, Message = "The request was canceled by the client." },
                UnauthorizedAccessException => new { StatusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized access." },
                _ => new { StatusCode = StatusCodes.Status500InternalServerError, Message = "An unexpected error occurred." }
            };

            _logger.LogError(exception, "An error occurred while processing the request: {Message}", exception.Message);

            var errorResponse = new
            {
                status = errorDetails.StatusCode,
                error = errorDetails.Message,
                timestamp = DateTime.UtcNow,
            };

            context.Response.StatusCode = errorDetails.StatusCode;
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
