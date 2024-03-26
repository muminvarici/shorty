using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shorty.Api.Domain.Entities;

namespace Shorty.Api.Infrastructure.Data.Configurations;

public class UrlUsageHistoryConfiguration : IEntityTypeConfiguration<UrlUsageHistory>
{
    public void Configure(EntityTypeBuilder<UrlUsageHistory> builder)
    {
        builder.HasKey(w => w.Id);

        builder.HasOne(w => w.UrlDetail)
            .WithMany()
            .HasForeignKey(w => w.UrlId);

        builder.Property(w => w.CreatedBy)
            .HasMaxLength(60);
    }
}