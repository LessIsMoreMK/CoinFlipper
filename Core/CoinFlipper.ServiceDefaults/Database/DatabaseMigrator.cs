using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoinFlipper.ServiceDefaults.Database;

public class DatabaseMigrator<TContext>(IServiceProvider serviceProvider) : IHostedService
    where TContext : DbContext
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}