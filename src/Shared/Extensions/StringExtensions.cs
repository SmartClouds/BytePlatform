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

    public static string? TrimIfHasValue(this string? value)
    {
        return value.HasNoValue()
            ? value
            : value!.Trim();
    }

    public static bool OrdinalEquals(this string? value, string? compareValue)
        => string.Equals(value, compareValue, StringComparison.Ordinal);

    public static bool OrdinalIgnoreCaseEquals(this string? value, string? compareValue) =>
        string.Equals(value, compareValue, StringComparison.OrdinalIgnoreCase);

    public static bool OrdinalEqualsOneOf(this string? value, params string?[] values)
    {
        if (values is null) return false;

        return Array.Exists(values, v => string.Equals(value, v, StringComparison.Ordinal));
    }

    public static bool OrdinalIgnoreCaseEqualsOneOf(this string? value, params string?[] values)
    {
        if (values is null) return false;

        return Array.Exists(values, v => string.Equals(value, v, StringComparison.OrdinalIgnoreCase));
    }

    public static string TruncateByLength(this string? value, int? length)
    {
        if (value.HasValue() is false)
            return string.Empty;

        if (length is null)
            return value;

        if (length <= 0)
            return string.Empty;

        if (value!.Length <= length)
            return value;

        return $"{value![..length.Value]}...";
    }

    public static bool OrdinalIgnoreCaseContains(this string? value, string compareValue)
        => value?.Contains(compareValue, StringComparison.OrdinalIgnoreCase) ?? false;
}
