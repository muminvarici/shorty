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
    private readonly ILogger<UrlService> _logger;
    private readonly Random _random = new();

    public UrlService
    (
        IHttpContextAccessor httpContextAccessor,
        CurrentUserService currentUserService,
        ApplicationDbContext dbContext,
        ILogger<UrlService> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
        _dbContext = dbContext;
        _logger = logger;
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
        var entity = new UrlDetail(request.Url, shortUrl, code, validUntil, DateTime.UtcNow, _currentUserService.GetUserIdentity(), request.IsSingleUsage, true);
        try
        {
            _dbContext.UrlDetails.Add(entity);
            await _dbContext.SaveChangesAsync();

            var response = new ShortUrlResponse(entity.LongUrl, entity.ShortUrl, entity.ValidUntil, entity.IsSingleUsage ?? false);
            _logger.LogInformation("Generated response @{response}", response);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> GenerateLongUrl(string code)
    {
        _logger.LogInformation("Checking code {code} by {user}", code, _currentUserService.GetUserIdentity());
        var urlDetail = await _dbContext.UrlDetails.SingleOrDefaultAsync(w => w.Code == code.ToLower() && w.IsActive);
        if (urlDetail == null) return null;

        if (urlDetail.IsSingleUsage == true)
        {
            urlDetail.IsActive = false;
        }

        _dbContext.UrlUsageHistories.Add(new UrlUsageHistory
        {
            UrlId = urlDetail.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUserService.GetUserIdentity()
        });

        await _dbContext.SaveChangesAsync();

        return urlDetail.LongUrl;
    }


    private bool ValidateRequest(ShortUrlRequest request)
    {
        _logger.LogInformation("Validating @{data}", request);
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