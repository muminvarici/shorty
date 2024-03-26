using Microsoft.EntityFrameworkCore;
using Shorty.Api.Domain.Constants;
using Shorty.Api.Domain.Entities;
using Shorty.Api.Infrastructure.Data;
using Shorty.Api.Presentation.Contracts.Requests;
using Shorty.Api.Presentation.Contracts.Responses;

namespace Shorty.Api.Application.Services;

public class UrlService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CurrentUserService _currentUserService;
    private readonly ApplicationDbContext _dbContext;
    private readonly Random _random = new();

    public UrlService(IHttpContextAccessor httpContextAccessor, CurrentUserService currentUserService, ApplicationDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
        _dbContext = dbContext;
    }

    public async Task<ShortUrlResponse?> GenerateShortUrl(ShortUrlRequest request)
    {
        if (!ValidateRequest(request)) return null;

        var codeLength = (request.CodeLength ?? UrlConstants.DefaultCodeLength);
        var chars = new char[codeLength];

        for (var index = 0; index < codeLength; index++)
        {
            var position = _random.Next(UrlConstants.Characters.Length - 1);
            chars[index] = UrlConstants.Characters[position];
        }

        var code = new string(chars);
        var shortUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/v1/url/{code}";
        var validUntil = request.LastUsageDate ?? DateTime.Now.AddYears(10);
        var entity = new UrlDetail(request.Url, shortUrl, code, validUntil, DateTime.UtcNow, _currentUserService.GetUserIdentity(), request.IsSingleUsage);
        _dbContext.UrlDetails.Add(entity);
        await _dbContext.SaveChangesAsync();

        return new ShortUrlResponse(entity.LongUrl, entity.ShortUrl, entity.ValidUntil, entity.IsSingleUsage ?? false);
    }

    public async Task<string?> GenerateLongUrl(string code)
    {
        var urlDetail = await _dbContext.UrlDetails.SingleOrDefaultAsync(w => w.Code == code);
        return urlDetail?.LongUrl;
    }


    private bool ValidateRequest(ShortUrlRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Url))
            return false;
        if (request.Url.Length > UrlConstants.MaxUrlLength)
            return false;
        if (request.CodeLength is < UrlConstants.MinCodeLength or > UrlConstants.MaxCodeLength)
            return false;
        if (request.LastUsageDate < DateTime.Now)
            return false;

        if (!request.Url.StartsWith("http://") && !request.Url.StartsWith("https://"))
            request.Url = $"https://{request.Url}";
        
        if (!Uri.TryCreate(request.Url, UriKind.RelativeOrAbsolute, out _))
            return false;

        return true;
    }
}