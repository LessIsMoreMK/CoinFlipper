using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.SwissArmy.Domain.Services;
using FluentValidation;

namespace CoinFlipper.SwissArmy.Application.Queries.PositionCalculator.Handlers;

public class GetPositionStatsSimpleHandler(
    IPositionCalculatorService positionCalculatorService
    ) : IQueryHandler<GetPositionStatsSimpleRequest, GetPositionStatsSimpleResponse>
{
    public async Task<GetPositionStatsSimpleResponse> HandleAsync(
        GetPositionStatsSimpleRequest query, CancellationToken cancellationToken = default)
    {
        var result = positionCalculatorService.CalculatePositionStatsSimple(query.Leverage, query.IsLong, query.EntryPrice, query.ExitPrice, query.Quantity);

        return new GetPositionStatsSimpleResponse(result.initialMargin, result.PNL, result.ROE);
    }
}

public class GetPositionStatsSimpleRequestValidator : AbstractValidator<GetPositionStatsSimpleRequest>
{
    public GetPositionStatsSimpleRequestValidator()
    {
        RuleFor(request => request.Leverage)
            .InclusiveBetween(1, 150);
    }
}