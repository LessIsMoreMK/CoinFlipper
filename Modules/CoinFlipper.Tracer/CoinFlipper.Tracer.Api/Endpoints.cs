using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.Tracer.Application.Queries.FearAndGreed;
using CoinFlipper.Tracer.Application.Queries.FearAndGreed.Handlers;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.Tracer.Api;

public static class Endpoints
{
    private const string BasePath = "tracer";
    
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints();
        app.MapInfrastructureEndpoints();
        
        app.MapFearAndGreedEndpoints();
        
        return app;
    }
    
    private static WebApplication MapFearAndGreedEndpoints(this WebApplication app)
    {
        app.MapGet($"/{BasePath}/fear-and-greed-index", async (IQueryDispatcher queryDispatcher, int limit) =>
        {
            var query = new GetFearAndGreedRequest { Limit = limit };
            var result = await queryDispatcher.QueryAsync<GetFearAndGreedRequest, GetFearAndGreedResponse>(query);
            return Results.Ok(result);
        });
        
        return app;
    }
    
    private static WebApplication MapInfrastructureEndpoints(this WebApplication app)
    {
        //TODO: Generic implementation
        app.MapPost($"/{BasePath}/migrate", async (HttpContext context) =>
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();
                
                await context.Response.WriteAsync("Migration completed.");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"Migration resulted in exception: {ex.Message}");
            }
        });
        
        return app;
    }
}