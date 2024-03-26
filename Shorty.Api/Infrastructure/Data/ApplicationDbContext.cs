using Microsoft.EntityFrameworkCore;
using Shorty.Api.Domain.Constants;
using Shorty.Api.Domain.Entities;

namespace Shorty.Api.Infrastructure.Data;

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
                .HasMaxLength(UrlConstants.MaxUrlLength)
                .IsRequired();

            builder.Property(w => w.ShortUrl)
                .HasMaxLength(UrlConstants.MaxUrlLength)
                .IsRequired();

            builder.Property(w => w.Code)
                .HasMaxLength(UrlConstants.MaxCodeLength)
                .IsRequired();

            builder.HasIndex(w => w.Code)
                .IsUnique();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}