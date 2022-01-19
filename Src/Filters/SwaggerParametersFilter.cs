using Blog.Controllers.Networks;
using Blog.Domain;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blog.Filters;

public class SwaggerParametersFilter : IParameterFilter
{
    public SwaggerParametersFilter() {}

    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if ((context.ParameterInfo.ParameterType == typeof(NetworkIn) ||
            context.ParameterInfo.ParameterType == typeof(DeleteNetworkIn)) &&
            parameter.Name.Equals(nameof(NetworkIn.Name), StringComparison.InvariantCultureIgnoreCase)
        ) {
            parameter.Schema.Enum = Network.Alloweds.Select(n => new OpenApiString(n)).ToList<IOpenApiAny>();
        }
    }
}
