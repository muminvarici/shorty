using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shorty.Api.Domain.Constants;
using Shorty.Api.Domain.Entities;

namespace Shorty.Api.Infrastructure.Data.Configurations;

public class UrlDetailConfiguration : IEntityTypeConfiguration<UrlDetail>
{
    public void Configure(EntityTypeBuilder<UrlDetail> builder)
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

        builder.Property(w => w.CreatedBy)
            .HasMaxLength(60);
    }
}