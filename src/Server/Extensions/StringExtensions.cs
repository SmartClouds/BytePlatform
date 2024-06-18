using System.Diagnostics.CodeAnalysis;

namespace BytePlatform.Server.Extensions;

public static class StringExtensions
{
    [return: NotNullIfNotNull(parameterName: "str")]
    public static string? CamelCase(this string? str)
    {
        if (string.IsNullOrEmpty(str) is false && str.Length > 1)
        {
            return char.ToLowerInvariant(str[0]) + str[1..];
        }
        return str;
    }
}
