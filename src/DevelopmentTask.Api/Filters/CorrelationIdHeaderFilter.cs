using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using DevelopmentTask.Api.Common;

namespace DevelopmentTask.Api.Filters
{
    public sealed class CorrelationIdHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (operation.Parameters.Any(p => p.Name == ApiConsts.CorrelationIdHeaderName))
                return;

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = ApiConsts.CorrelationIdHeaderName,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "uuid",
                    Default = new OpenApiString(Guid.NewGuid().ToString())
                },
                Description = "Correlation ID for tracking requests"
            });
        }
    }
}
