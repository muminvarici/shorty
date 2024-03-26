namespace Shorty.Api.Domain.Entities;

public class UrlUsageHistory : EntityBase
{
    public Guid Id { get; set; }
    public int UrlId { get; set; }

    public UrlDetail UrlDetail { get; set; }
}