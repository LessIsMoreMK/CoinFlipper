using CoinFlipper.ServiceDefaults.Options;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace CoinFlipper.ServiceDefaults.Logging;

public static class LoggingExtensions
{
    #region Methods
    
    public static IHostApplicationBuilder AddCustomLogging(this IHostApplicationBuilder builder)
    {
        var loggerOptions = builder.Services.GetOptions<LoggerOptions>("Logger");
        
        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .MinimumLevel.Is(Enum.Parse<LogEventLevel>(loggerOptions.Level));

        if (loggerOptions.Console.Enabled)
            loggerConfig.WriteTo.Console();

        if (loggerOptions.File.Enabled)
            loggerConfig.WriteTo.File(
                new RenderedCompactJsonFormatter(),
                loggerOptions.File.Path,
                rollingInterval: ParseRollingInterval(loggerOptions.File.Interval));

        if (loggerOptions.Seq.Enabled)
            loggerConfig.WriteTo.Seq(
                loggerOptions.Seq.Url,
                apiKey: loggerOptions.Seq.ApiKey);

        foreach (var overrideConfig in loggerOptions.MinimumLevelOverrides)
            loggerConfig.MinimumLevel.Override(overrideConfig.Key, Enum.Parse<LogEventLevel>(overrideConfig.Value));

        builder.Logging.AddSerilog(loggerConfig.CreateLogger());

        return builder;
    }
    
    #endregion
    
    #region Private Helpers
    
    private static RollingInterval ParseRollingInterval(string interval)
    {
        return interval.ToLower() switch
        {
            "day" => RollingInterval.Day,
            "hour" => RollingInterval.Hour,
            "minute" => RollingInterval.Minute,
            "month" => RollingInterval.Month,
            "year" => RollingInterval.Year,
            _ => RollingInterval.Infinite, 
        };
    }
    
    #endregion
}