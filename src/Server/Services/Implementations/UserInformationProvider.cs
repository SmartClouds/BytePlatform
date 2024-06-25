using System.Security.Claims;
using BytePlatform.Server.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace BytePlatform.Server.Services.Implementations;

public partial class UserInformationProvider<TKey> : IUserInformationProvider<TKey>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInformationProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IEnumerable<Claim> GetClaims()
    {
        if (IsAuthenticated() is false)
        {
            throw new InvalidOperationException();
        }

        return GetClaimsIdentity().Claims;
    }

    public ClaimsIdentity GetClaimsIdentity()
    {
        if (IsAuthenticated() is false)
        {
            throw new InvalidOperationException();
        }

        return (ClaimsIdentity)_httpContextAccessor.HttpContext!.User.Identity!;
    }

    public TKey GetUserId()
    {
        if (IsAuthenticated() is false)
        {
            throw new InvalidOperationException();
        }

        return _httpContextAccessor.HttpContext!.User.GetUserId<TKey>();
    }

    public string? GetUserName()
    {
        if (IsAuthenticated() is false)
        {
            throw new InvalidOperationException();
        }

        return _httpContextAccessor.HttpContext!.User.Identity?.Name;
    }

    public bool IsAuthenticated()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }

        return _httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated is true;
    }

    public bool IsInRole(string role)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException();
        }

        return _httpContextAccessor.HttpContext.User?.IsInRole(role) is true;
    }
}
