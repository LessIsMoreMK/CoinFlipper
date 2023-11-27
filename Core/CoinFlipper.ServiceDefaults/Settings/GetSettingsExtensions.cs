using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.ServiceDefaults.Settings;

public static class GetSettingsExtensions
{
    public static TModel GetSettings<TModel>(this IServiceCollection serviceCollection, string settingsSectionName) where TModel : class, new()
    {
        using var provider = serviceCollection.BuildServiceProvider();
        return provider.GetSettings<TModel>(settingsSectionName);
    }

    public static TModel GetSettings<TModel>(this IServiceProvider serviceProvider, string settingsSectionName) where TModel : class, new()
    {
        return (serviceProvider.GetService<IConfiguration>() ?? throw new InvalidOperationException())
            .GetSettings<TModel>(settingsSectionName);
    }
    
    public static TModel GetSettings<TModel>(this IConfiguration configuration, string settingsSectionName) where TModel : class, new()
    {
        var instance = new TModel();
        configuration.GetSection(settingsSectionName).Bind(instance);
        return instance;
    }
}