namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static T GetUserId<T>(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.GetUserId();
        if (typeof(T) == typeof(Guid))
        {
            return (T)Convert.ChangeType(Guid.Parse(userId), typeof(T));
        }
        return (T)Convert.ChangeType(userId, typeof(T));
    }

    public static string? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return (claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) ?? claimsPrincipal.FindFirst("nameid"))!.Value;
    }

    public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
    {
        return (claimsPrincipal.FindFirst(ClaimTypes.Name) ?? claimsPrincipal.FindFirst("unique_name"))!.Value;
    }

    public static bool IsAuthenticated(this ClaimsPrincipal? claimsPrincipal)
    {
        return claimsPrincipal?.Identity?.IsAuthenticated is true;
    }
}
