namespace Shorty.Api.Presentation.Contracts.Responses;

public record ShortUrlResponse(string LongUrl, string ShortUrl, DateTime ValidUntil, bool IsSingleUsage);