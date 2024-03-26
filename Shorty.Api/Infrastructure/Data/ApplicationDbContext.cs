using Microsoft.EntityFrameworkCore;
using Shorty.Api.Domain.Entities;
using Shorty.Api.Infrastructure.Data.Configurations;

namespace Shorty.Api.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<UrlDetail> UrlDetails { get; set; }
    public DbSet<UrlUsageHistory> UrlUsageHistories { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UrlDetailConfiguration());
        modelBuilder.ApplyConfiguration(new UrlUsageHistoryConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}