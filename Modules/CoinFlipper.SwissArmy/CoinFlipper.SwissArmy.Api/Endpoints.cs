using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.SwissArmy.Application.Queries.PositionCalculator;
using CoinFlipper.SwissArmy.Application.Queries.Sentence;
using CoinFlipper.SwissArmy.Domain.Entities;
using CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.SwissArmy.Api;

public static class Endpoints
{
    private const string BasePath = "swissArmy";
    
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints().
            MapInfrastructureEndpoints()
            .MapPositionCalculatorEndpoints()
            .MapSentenceEndpoints()
            
            ;
        
        return app;
    }
    
    private static WebApplication MapPositionCalculatorEndpoints(this WebApplication app)
    {
        app.MapGet($"/{BasePath}/position-stats/simple", 
            async (IQueryDispatcher queryDispatcher, int leverage, bool isLong, double entryPrice, double exitPrice, double quantity) =>
        {
            var query = new GetPositionStatsSimpleRequest() 
                { Leverage = leverage, IsLong = isLong, EntryPrice = entryPrice, ExitPrice = exitPrice, Quantity = quantity};
            var result = await queryDispatcher.QueryAsync<GetPositionStatsSimpleRequest, GetPositionStatsSimpleResponse>(query);
            return Results.Ok(result);
        });
        
        return app;
    }
    
    private static WebApplication MapSentenceEndpoints(this WebApplication app)
    {
        app.MapGet($"/{BasePath}/sentences", async (IQueryDispatcher queryDispatcher, int limit) =>
        {
            var query = new GetSentencesRequest() { Limit = limit };
            var result = await queryDispatcher.QueryAsync<GetSentencesRequest, List<SentenceEntity>>(query);
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