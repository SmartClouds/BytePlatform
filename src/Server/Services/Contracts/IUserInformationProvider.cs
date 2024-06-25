using System.Security.Claims;

namespace BytePlatform.Server.Services.Contracts;

public partial interface IUserInformationProvider<TKey>
{
    bool IsAuthenticated();

    IEnumerable<Claim> GetClaims();

    ClaimsIdentity GetClaimsIdentity();

    TKey GetUserId();

    string? GetUserName();

    bool IsInRole(string role);
}
