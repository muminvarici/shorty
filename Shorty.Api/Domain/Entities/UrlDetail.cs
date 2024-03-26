namespace Shorty.Api.Domain.Entities;

public class UrlDetail : EntityBase
{
    public UrlDetail()
    {
    }

    public UrlDetail(string longUrl, string shortUrl, string code, DateTime validUntil, DateTime createdAt, string createdBy, bool? isSingleUsage, bool isActive)
    {
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        Code = code;
        ValidUntil = validUntil;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        IsSingleUsage = isSingleUsage;
        IsActive = isActive;
    }

    public int Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public bool? IsSingleUsage { get; set; }
    public bool IsActive { get; set; }
    public DateTime ValidUntil { get; set; }
}