using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Shared.Infrastructure.Configs.Swagger;

public static class SwaggerUIConfig
{
    public static void ConfigureSwaggerUI(SwaggerUIOptions c)
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clothing Store API v1");

        // Optional: customize UI
        c.DocumentTitle = "Clothing Store API";
        c.RoutePrefix = string.Empty; 
    }
}
