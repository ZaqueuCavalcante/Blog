using System.Security.Claims;
using Blog.Exceptions;

namespace Blog.Extensions
{
    public static class Extensions
    {
        public static string Format(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }

        public static int GetId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirstValue("sub"));
        }

        public static string GetRoot(this HttpRequest request)
        {
            return request.Scheme + "://" + request.Host.Value + "/";
        }

        public static IApplicationBuilder UseDomainExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<DomainExceptionMiddleware>();
            return app;
        }
    }
}
