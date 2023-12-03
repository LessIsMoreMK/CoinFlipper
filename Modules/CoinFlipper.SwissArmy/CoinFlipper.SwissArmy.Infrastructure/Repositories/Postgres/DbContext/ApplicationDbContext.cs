using CoinFlipper.SwissArmy.Infrastructure.Repositories.Models;
using CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinFlipper.SwissArmy.Infrastructure.Repositories.Postgres.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Properties
    
    public DbSet<SentenceDb> Sentence { get; set; } = null!;

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
        modelBuilder.ApplyConfiguration(new SentenceConfiguration());
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