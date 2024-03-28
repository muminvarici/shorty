namespace Shorty.Api.Application.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
{
    private string? _identity;

    public string GetUserIdentity()
    {
        if (!string.IsNullOrWhiteSpace(_identity)) return _identity;

        var context = httpContextAccessor.HttpContext;
        var success = context!.Request.Headers.TryGetValue("X-Real-IP", out var ipAddress) &&
                      context.Request.Headers.TryGetValue("X-Forwarded-For", out ipAddress);

        if (success && !string.IsNullOrWhiteSpace(ipAddress))
        {
            _identity = ipAddress;
            return ipAddress!;
        }

        _identity = context.User.Identity?.Name
                    ?? context.Connection.RemoteIpAddress?.ToString()
                    ?? "system";

        return _identity;
    }
}