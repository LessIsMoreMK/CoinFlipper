using CoinFlipper.ServiceDefaults;
using CoinFlipper.ServiceDefaults.Application;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Tracer.Application;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.AddApplicationBase();
        builder.AddLoggingDecorators();


        return builder;
    }
}