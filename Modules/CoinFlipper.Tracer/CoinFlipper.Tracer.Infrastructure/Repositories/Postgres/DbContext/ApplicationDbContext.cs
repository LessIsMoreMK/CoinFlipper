using CoinFlipper.Tracer.Infrastructure.Repositories.Models;
using CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFlipper.Tracer.Infrastructure.Repositories.Postgres.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Properties
    
    public DbSet<FearAndGreedDb> FearAndGreed { get; set; } = null!;

    #endregion
    
    #region Constructors

    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    #endregion
    
    #region Methods
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FearAndGreedConfiguration());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName))
            .EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }
    
    #endregion
}