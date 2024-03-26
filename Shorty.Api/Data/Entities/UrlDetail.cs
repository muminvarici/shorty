namespace Shorty.Api.Entities;

public class UrlDetail
{
    public UrlDetail()
    {
    }

    public UrlDetail(string longUrl, string shortUrl, string code, DateTime validUntil, DateTime createdAt, string createdBy, bool? isSingleUsage)
    {
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        Code = code;
        ValidUntil = validUntil;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        IsSingleUsage = isSingleUsage;
    }

    public int Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;
    public bool? IsSingleUsage { get; }
    public DateTime ValidUntil { get; set; }
    public DateTime CreatedAt { get; set; }
}