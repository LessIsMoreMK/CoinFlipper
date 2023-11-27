using CoinFlipper.ServiceDefaults.Application.Commands;
using CoinFlipper.ServiceDefaults.Application.Events;
using CoinFlipper.ServiceDefaults.Application.Queries;
using CoinFlipper.ServiceDefaults.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.ServiceDefaults.Application;

public static class ApplicationExtensions
{
    #region Methods
    
    public static IHostApplicationBuilder AddApplicationBase(this IHostApplicationBuilder builder)
    {
        return builder
            .AddCommandHandlers()
            .AddInMemoryCommandDispatcher()
            .AddEventHandlers()
            .AddInMemoryEventDispatcher()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher();
    }
    
    #endregion
    
    #region Private Helpers
    
    private static IHostApplicationBuilder AddCommandHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))
                    .WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return builder;
    }
    
    private static IHostApplicationBuilder AddInMemoryCommandDispatcher(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        return builder;
    }
    
    private static IHostApplicationBuilder AddEventHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                    .WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return builder;
    }
    
    private static IHostApplicationBuilder AddInMemoryEventDispatcher(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
        return builder;
    }
    
    private static IHostApplicationBuilder AddQueryHandlers(this IHostApplicationBuilder builder)
    {
        builder.Services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))
                    .WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return builder;
    }
    
    private static IHostApplicationBuilder AddInMemoryQueryDispatcher(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        return builder;
    }
    
    #endregion
}