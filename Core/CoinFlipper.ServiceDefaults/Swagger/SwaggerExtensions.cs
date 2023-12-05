using CoinFlipper.ServiceDefaults.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CoinFlipper.ServiceDefaults.Swagger;

public static class SwaggerExtensions
{
    private const string OptionsSectionName = "Swagger";
    
    public static IHostApplicationBuilder AddSwagger(this IHostApplicationBuilder builder)
    {
        var swaggerOptions = builder.Services.GetOptions<SwaggerOptions>(OptionsSectionName);

        if (!swaggerOptions.Enabled)
            return builder;
        
        builder.Services.AddMvcCore();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerOptions.Name, 
                new OpenApiInfo { Title = swaggerOptions.Title, Version = swaggerOptions.Version });
        });
        
        return builder;
    }
	
    public static WebApplication UseSwagger(this WebApplication app)
    {
        var swaggerOptions = app.Services.GetOptions<SwaggerOptions>(OptionsSectionName);

        if (!swaggerOptions.Enabled)
            return app;
        
        SwaggerBuilderExtensions.UseSwagger(app);
        app.UseSwaggerUI();

        return app;
    }
}