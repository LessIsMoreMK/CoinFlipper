using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinFlipper.ServiceDefaults.Options;

public static class GetOptionsExtensions
{
    public static TModel GetOptions<TModel>(this IServiceCollection serviceCollection, string optionsSectionName) where TModel : class, new()
    {
        using var provider = serviceCollection.BuildServiceProvider();
        return provider.GetOptions<TModel>(optionsSectionName);
    }

    public static TModel GetOptions<TModel>(this IServiceProvider serviceProvider, string optionsSectionName) where TModel : class, new()
    {
        return (serviceProvider.GetService<IConfiguration>() ?? throw new InvalidOperationException())
            .GetOptions<TModel>(optionsSectionName);
    }
    
    public static TModel GetOptions<TModel>(this IConfiguration configuration, string optionsSectionName) where TModel : class, new()
    {
        var instance = new TModel();
        configuration?.GetSection(optionsSectionName).Bind(instance);
        return instance;
    }
}