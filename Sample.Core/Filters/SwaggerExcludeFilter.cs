using Sample.WebAPI.Core.Attributes;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sample.WebAPI.Core.Filters
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var debug = context.Type.GetProperties();
            var excludedProperties = context.Type.GetProperties()
                .Where(x => x.GetCustomAttributes(true).OfType<SwaggerExcludeAttribute>().Any())
                .Select(x => x.Name)
                .ToList();

            foreach (var excludedProperty in excludedProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty))
                    schema.Properties.Remove(excludedProperty);
            }
        }
    }
}
