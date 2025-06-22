using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ClienteAPI_
{
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
                return;

            var ignoredProperties = context.Type.GetProperties()
                .Where(prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                .Select(prop => prop.Name.ToLower());

            foreach (var propertyName in ignoredProperties)
            {
                schema.Properties.Remove(propertyName);
            }
        }
    }
}
