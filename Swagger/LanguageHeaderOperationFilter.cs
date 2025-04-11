using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExamApi.Swagger
{
    public class LanguageHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Langue à utiliser pour les messages (fr ou en)",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("fr")
                }
            });
        }
    }

}
