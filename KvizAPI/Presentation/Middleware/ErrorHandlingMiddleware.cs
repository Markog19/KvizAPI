namespace KvizAPI.Presentation.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate _next, ILogger<ErrorHandlingMiddleware> _logger)
    {

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught by middleware.");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = new
                {
                    ex.Message,
                    Details = ex.InnerException.Message ?? string.Empty
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
