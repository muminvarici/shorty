namespace Shorty.Api.Application.Services;

public class CurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserIdentity()
    {
        var context = _httpContextAccessor.HttpContext;
        var success = context.Request.Headers.TryGetValue("X-Real-IP", out var ipAddress) &&
                      context.Request.Headers.TryGetValue("X-Forwarded-For", out ipAddress);

        if (success && !string.IsNullOrWhiteSpace(ipAddress))
        {
            return ipAddress;
        }

        return context.User.Identity?.Name
               ?? context.Connection.RemoteIpAddress?.ToString()
               ?? "system";
    }
}