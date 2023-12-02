using CoinFlipper.Notification.Application.Services.Email;
using CoinFlipper.Notification.Infrastructure.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.Notification.Infrastructure;

public static class Extensions
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<IEmailSender, SendGridEmailSender>()
            .AddSingleton<IEmailTemplateSender, EmailTemplateSender>()
            .AddSingleton<IApplicationEmailSender, ApplicationEmailSender>()
            
            ;

        return services;
    }
    
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {

        return builder;
    }
}