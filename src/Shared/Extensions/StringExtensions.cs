namespace BytePlatform.Shared.Extensions;

public static class StringExtensions
{
    public static string? TrimIfNotNull(this string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }
        return input.Trim();
    }

    public static bool? IsNullOrEmpty(this string? input)
        => string.IsNullOrEmpty(input);

    public static bool? IsNullOrWhiteSpace(this string? input)
        => string.IsNullOrWhiteSpace(input);

    public static bool HasValue(this string? value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace
            ? string.IsNullOrWhiteSpace(value) is false
            : string.IsNullOrEmpty(value) is false;
    }

    public static bool HasNoValue(this string? value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace
            ? string.IsNullOrWhiteSpace(value)
            : string.IsNullOrEmpty(value);
    }
}
