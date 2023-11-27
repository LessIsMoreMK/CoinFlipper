using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.Tracer.Application.Dtos;
using CoinFlipper.Tracer.Application.Queries.FearAndGreed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CoinFlipper.Tracer.Api;

public static class Endpoints
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDefaultEndpoints();

        app.MapGet("/fear-greed-index", async (IQueryDispatcher queryDispatcher, int limit) =>
        {
            var query = new GetFearAndGreedRequest { Limit = limit };
            var result = await queryDispatcher.QueryAsync<GetFearAndGreedRequest, List<FearAndGreedDto>>(query);
            return Results.Ok(result);
        });
        
        return app;
    }
}