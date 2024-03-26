namespace Shorty.Api.Responses;

public record ShortUrlResponse(string LongUrl, string ShortUrl, DateTime ValidUntil, bool IsSingleUsage);