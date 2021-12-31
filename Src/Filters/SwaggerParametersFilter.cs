using Blog.Controllers.Posts;
using Blog.Controllers.Networks;
using Blog.Database;
using Blog.Domain;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blog.Filters
{
    public class SwaggerParametersFilter : IParameterFilter
    {
        readonly IServiceScopeFactory _serviceScopeFactory;

        public SwaggerParametersFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (context.ParameterInfo.ParameterType == typeof(NetworkIn) &&
                parameter.Name.Equals(nameof(NetworkIn.Name), StringComparison.InvariantCultureIgnoreCase)
            ) {
                parameter.Schema.Enum = Network.Alloweds.Select(n => new OpenApiString(n)).ToList<IOpenApiAny>();
            }

            if (context.ParameterInfo.ParameterType == typeof(PostParameters) &&
                parameter.Name.Equals(nameof(PostParameters.Tag), StringComparison.InvariantCultureIgnoreCase)
            ) {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var blogContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
                    var tags = blogContext.Tags.Select(t => t.Name).ToList();
                    parameter.Schema.Enum = tags.Select(t => new OpenApiString(t)).ToList<IOpenApiAny>();
                }
            }
        }
    }
}
