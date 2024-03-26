using Microsoft.EntityFrameworkCore;
using Shorty.Api.Entities;

namespace Shorty.Api.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<UrlDetail> UrlDetails { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlDetail>((builder) =>
        {
            builder.HasKey(w => w.Id);
            builder.Property(w => w.LongUrl)
                .HasMaxLength(Constants.UrlConstants.MaxUrlLength)
                .IsRequired();

            builder.Property(w => w.ShortUrl)
                .HasMaxLength(Constants.UrlConstants.MaxUrlLength)
                .IsRequired();

            builder.Property(w => w.Code)
                .HasMaxLength(Constants.UrlConstants.MaxCodeLength)
                .IsRequired();

            builder.HasIndex(w => w.Code)
                .IsUnique();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}