using System.Net;
using System.Text.Json;

namespace Blog.Exceptions
{
    public class DomainExceptionMiddleware
    {
        readonly RequestDelegate next;
        public DomainExceptionMiddleware(RequestDelegate next) => this.next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (DomainException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, DomainException ex)
        {
            var result = JsonSerializer.Serialize(new { error = ex.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(result);
        }
    }
}
