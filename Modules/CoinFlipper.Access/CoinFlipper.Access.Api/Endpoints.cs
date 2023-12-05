using CoinFlipper.ServiceDefaults;
using Microsoft.AspNetCore.Builder;

namespace CoinFlipper.Access.Api;

public static class Endpoints
{
    private const string BasePath = "access";
    
    internal static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints()
            
            ;
        
        return app;
    }
}