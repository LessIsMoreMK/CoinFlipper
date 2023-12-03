using CoinFlipper.Notification.Application.Services.Email;
using CoinFlipper.Notification.Infrastructure.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Notification.Infrastructure;

public static class Extensions
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddSingleton<IEmailSender, SendGridEmailSender>()
            .AddSingleton<IEmailTemplateSender, EmailTemplateSender>()
            .AddSingleton<IApplicationEmailSender, ApplicationEmailSender>();
            
            
        return builder;
    }
}