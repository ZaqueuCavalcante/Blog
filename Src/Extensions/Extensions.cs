using System.Security.Claims;
using Blog.Controllers;
using Newtonsoft.Json;

namespace Blog.Extensions;

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
        return "https://" + request.Host.Value + "/";
    }

    public static void AddPagination(
        this HttpResponse response,
        RequestParameters parameters,
        int count
    ) {
        var metadata = new Metadata
        {
            TotalCount = count,
            PageSize = parameters.PageSize,
            CurrentPage = parameters.PageNumber,
            TotalPages = (int) Math.Ceiling(count / (double) parameters.PageSize)
        };

        response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
    }

    public static IQueryable<TSource> Page<TSource>(
        this IQueryable<TSource> source,
        RequestParameters parameters
    ) {
        return source
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize);
    }
}
