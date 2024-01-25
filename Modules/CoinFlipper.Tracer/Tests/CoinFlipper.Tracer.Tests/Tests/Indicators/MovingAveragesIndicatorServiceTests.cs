using CoinFlipper.Tracer.Application.Services.Indicators;
using CoinFlipper.Tracer.Domain.Entities;
using CoinFlipper.Tracer.Domain.Services;
using CoinFlipper.Tracer.Tests.Builders;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;

namespace CoinFlipper.Tracer.Tests.Tests.Indicators;

public class MovingAveragesIndicatorServiceTests
{
    //NOTE: Expected results are checked with external sources 
    //TODO: Tests for HMA, SMMA
    
    #region Setup
    
    private readonly MovingAverageIndicatorService _movingAverageIndicatorService;
    private readonly Mock<IRedisCacheService> _redisCacheService;
    private readonly Guid CoinId = new Guid("176de950-d825-4a94-95cc-311c567b92e0");
    private const string CoinSymbol = "BTC";

    public MovingAveragesIndicatorServiceTests()
    {
        var logger = NullLogger<MovingAverageIndicatorService>.Instance;
        _redisCacheService = new Mock<IRedisCacheService>();
        _movingAverageIndicatorService = new MovingAverageIndicatorService(logger, _redisCacheService.Object);
    }
    
    #endregion
    
    #region SMA
    
    public static IEnumerable<object[]> SMAInvalidTestData()
    {
        // Too much records
        yield return new object[] 
        { 
            1, null!,
            CoinDataBuilder.Multiple(2)
        };
        
        // Not enough records
        yield return new object[] 
        { 
            3, null!,
            CoinDataBuilder.Multiple(2)
        };
        
        // Incorrect DateTime
        yield return new object[] 
        { 
            3, null!,
            CoinDataBuilder.Multiple(2)
                .Concat(new []{CoinDataBuilder.SingleWithDateTime(new DateTime(2023, 12, 10, 0, 0, 0, DateTimeKind.Utc))})
                .ToList()
        };
    }
    
    [Theory]
    [MemberData(nameof(SMAInvalidTestData))]
    public async Task CalculateSMA_WithInValidData_ReturnNullValue(int length, decimal? expectedResult, List<CoinData> coinData)
    {
        _redisCacheService
            .Setup(x => x.GetCoinDataListAsync(CoinId, length))
            .ReturnsAsync(coinData);
    
        var result = await _movingAverageIndicatorService.CalculateSMA(length, CoinId, CoinSymbol);
    
        result.ShouldBeNull();
        result.ShouldBe(expectedResult);
    }
    
    public static IEnumerable<object[]> SMAValidTestData()
    {
        yield return new object[] 
        { 
            1, 1,
            CoinDataBuilder.TestData1()
        };
        
        yield return new object[] 
        { 
            3, 2,
            CoinDataBuilder.TestData2()
        };
        
        yield return new object[] 
        { 
            3, 1.5,
            CoinDataBuilder.TestData3()
        };
        
        yield return new object[] 
        { 
            10, 14.313152868,
            CoinDataBuilder.TestData4()
        };
    }
    
    [Theory]
    [MemberData(nameof(SMAValidTestData))]
    public async Task CalculateSMA_WithValidData_ReturnCorrectValue(int length, decimal expectedResult, List<CoinData> coinData)
    {
        _redisCacheService
            .Setup(x => x.GetCoinDataListAsync(CoinId, length))
            .ReturnsAsync(coinData);
        
        var result = await _movingAverageIndicatorService.CalculateSMA(length, CoinId, CoinSymbol, false);
        
        result.ShouldNotBeNull();
        result.ShouldBe(expectedResult);
    }
    
    #endregion
    
    #region EMA
    
    public static IEnumerable<object[]> EMAValidTestData()
    {
        yield return new object[] 
        { 
            1, 1,
            CoinDataBuilder.TestData1()
        };
        
        yield return new object[] 
        { 
            3, 2.25,
            CoinDataBuilder.TestData2()
        };
        
        yield return new object[] 
        { 
            3, 1.5975,
            CoinDataBuilder.TestData3()
        };
        
        yield return new object[] 
        { 
            10, 14.796319206222417399250100666m,
            CoinDataBuilder.TestData4()
        };
    }
    
    [Theory]
    [MemberData(nameof(EMAValidTestData))]
    public async Task CalculateEMA_WithValidData_ReturnCorrectValue(int length, decimal expectedResult, List<CoinData> coinData)
    {
        _redisCacheService
            .Setup(x => x.GetCoinDataListAsync(CoinId, length))
            .ReturnsAsync(coinData);
        
        var result = await _movingAverageIndicatorService.CalculateEMA(length, CoinId, CoinSymbol, false);
        
        result.ShouldNotBeNull();
        result.ShouldBe(expectedResult);
    }
    
    #endregion
    
    #region VWAP
    
    public static IEnumerable<object[]> VWAPValidTestData()
    {
        yield return new object[] 
        { 
            1, 1,
            CoinDataBuilder.TestData1()
        };
        
        yield return new object[] 
        { 
            3, 2.3333333333333333333333333333m,
            CoinDataBuilder.TestData2()
        };
        
        yield return new object[] 
        { 
            3, 1.7041463414634146341463414634m,
            CoinDataBuilder.TestData3()
        };
        
        yield return new object[] 
        { 
            10, 12.473149928461106655974338412m,
            CoinDataBuilder.TestData4()
        };
    }
    
    [Theory]
    [MemberData(nameof(VWAPValidTestData))]
    public async Task CalculateVWAP_WithValidData_ReturnCorrectValue(int length, decimal expectedResult, List<CoinData> coinData)
    {
        _redisCacheService
            .Setup(x => x.GetCoinDataListAsync(CoinId, length))
            .ReturnsAsync(coinData);
        
        var result = await _movingAverageIndicatorService.CalculateVWAP(length, CoinId, CoinSymbol, false);
        
        result.ShouldNotBeNull();
        result.ShouldBe(expectedResult);
    }
    
    #endregion
    
    #region WMA
    
    public static IEnumerable<object[]> WMAValidTestData()
    {
        yield return new object[] 
        { 
            1, 1,
            CoinDataBuilder.TestData1()
        };
        
        yield return new object[] 
        { 
            3, 1.6666666666666666666666666667m,
            CoinDataBuilder.TestData2()
        };
        
        yield return new object[] 
        { 
            3, 1.3933333333333333333333333334m,
            CoinDataBuilder.TestData3()
        };
        
        yield return new object[] 
        { 
            10, 12.461001323836363636363636365m,
            CoinDataBuilder.TestData4()
        };
    }
    
    [Theory]
    [MemberData(nameof(WMAValidTestData))]
    public async Task CalculateWMA_WithValidData_ReturnCorrectValue(int length, decimal expectedResult, List<CoinData> coinData)
    {
        _redisCacheService
            .Setup(x => x.GetCoinDataListAsync(CoinId, length))
            .ReturnsAsync(coinData);
        
        var result = await _movingAverageIndicatorService.CalculateWMA(length, CoinId, CoinSymbol, false);
        
        result.ShouldNotBeNull();
        result.ShouldBe(expectedResult);
    }
    
    #endregion
}