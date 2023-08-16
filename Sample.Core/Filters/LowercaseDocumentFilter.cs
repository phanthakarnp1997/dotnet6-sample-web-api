using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Filters
{
    public class LowercaseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Collect the existing paths and their corresponding values
            var paths = swaggerDoc.Paths.ToList();

            // Clear the original paths in the document
            swaggerDoc.Paths.Clear();

            // Update the route templates to lowercase
            foreach (var path in paths)
            {
                var lowercasePath = path.Key.ToLowerInvariant();
                var existingPath = swaggerDoc.Paths.FirstOrDefault(p => p.Key.ToLowerInvariant() == lowercasePath);

                if (existingPath.Key == null)
                {
                    // If the lowercasePath does not already exist, add it to the document
                    swaggerDoc.Paths[lowercasePath] = path.Value;
                }
                else
                {
                    // If lowercasePath already exists, merge the contents of the duplicate keys if necessary.
                    // For simplicity, this example will just log a warning message.
                    // Alternatively, you can choose to skip or handle duplicates in a way that suits your needs.
                    Console.WriteLine($"Warning: Duplicate key '{lowercasePath}' found in the Swagger paths.");
                }
            }
        }
    }


}
