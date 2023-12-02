using CoinFlipper.ServiceDefaults;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Notification.Application;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.AddApplicationBase();
        builder.AddLoggingDecorators();


        return builder;
    }
}