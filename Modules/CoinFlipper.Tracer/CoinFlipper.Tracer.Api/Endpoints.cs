using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.Tracer.Application.Dtos;
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
        app.MapFearAndGreedEndpoints();
        app.InitializeDatabase();
        
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
    
    private static WebApplication InitializeDatabase(this WebApplication app)
    {
        app.MapPost($"/{BasePath}/migrate", async () =>
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
            
            return Results.Ok();
        });
        
        return app;
    }
}