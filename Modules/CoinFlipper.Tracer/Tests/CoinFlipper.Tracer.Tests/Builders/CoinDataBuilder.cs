using BuilderGenerator;
using CoinFlipper.Tracer.Domain.Entities;

namespace CoinFlipper.Tracer.Tests.Builders;

//For more information see https://www.nuget.org/packages/BuilderGenerator

[BuilderFor(typeof(CoinData))]
public partial class CoinDataBuilder
{
    public static CoinDataBuilder Single()
    {
        return new CoinDataBuilder();
    }
    
    public static CoinData SingleWithDateTime(DateTime dateTime)
    {
        return new CoinDataBuilder()
            .WithDateTime(dateTime)
            .Build();
    }

    public static List<CoinData> Multiple(int count)
    {
        var coinDataList = new List<CoinData>();
        var dateTime = new DateTime(2023, 12, 1, 0, 0, 0, DateTimeKind.Utc);

        for (var i = 0; i < count; i++)
        {
            coinDataList.Add(
                new CoinDataBuilder()
                    .WithDateTime(dateTime)
                    .Build());

            dateTime = dateTime.AddMinutes(5);
        }

        return coinDataList;
    }
    
    
    #region Specific
    
    public static List<CoinData> TestData1()
    {
        var coinDataList = new List<CoinData>();
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(1)
                .WithVolume(1)
                .Build());

        return coinDataList;
    }
    
    public static List<CoinData> TestData2()
    {
        var coinDataList = new List<CoinData>();
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(1)
                .WithVolume(1)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(2)
                .WithVolume(2)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(3)
                .WithVolume(3)
                .Build());

        return coinDataList;
    }
    
    public static List<CoinData> TestData3()
    {
        var coinDataList = new List<CoinData>();
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(1.25m)
                .WithVolume(123)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(1.36m)
                .WithVolume(24)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(1.89m)
                .WithVolume(345)
                .Build());

        return coinDataList;
    }
    
    public static List<CoinData> TestData4()
    {
        var coinDataList = new List<CoinData>();
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(5.223213125m)
                .WithVolume(761)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(1.312312336m)
                .WithVolume(356)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(20.81233239m)
                .WithVolume(333)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(11.623122332m)
                .WithVolume(672)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(39.221313321m)
                .WithVolume(234)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(2.131233123m)
                .WithVolume(644)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(9.531223213m)
                .WithVolume(411)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(15.91233233m)
                .WithVolume(501)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(4.2412332m)
                .WithVolume(521)
                .Build());
        
        coinDataList.Add(
            new CoinDataBuilder()
                .WithPrice(33.12321331m)
                .WithVolume(555)
                .Build());

        return coinDataList;
    }
    
    #endregion
}
