using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OperationManagementService.Filters
{
    public class AuthenticationKeyHeader : Attribute, IOperationFilter
    {
        // Add the AuthenticationKey header to all operations
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "AuthenticationKey",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "uuid",
                    Example = new OpenApiString("123e4567-e89b-12d3-a456-426614174000")
                },
                Description = "API Key for authentication"
            });
        }
    }
}
