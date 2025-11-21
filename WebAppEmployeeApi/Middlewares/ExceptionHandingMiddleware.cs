using System.Net;
using System.Text.Json;

namespace WebAppEmployeeApi.Middlewares
{
    public class ExceptionHandingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    message = "An unexpected error occured",
                    details = ex.Message
                };

                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
