using System.ComponentModel.DataAnnotations;

namespace BytePlatform.Tiny.Shared.Extensions;
public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (field is null)
        {
            return string.Empty;
        }

        if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
        {
            return attribute.Name ?? enumValue.ToString();
        }

        return enumValue.ToString();
    }

    public static T FromInt<T>(int value)
    {
        return (T)(object)value;
    }

    public static T FromString<T>(this string value) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    public static (T? MinValue, T? MaxValue) GetMinAndMaxValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        var max = values.Max();
        var min = values.Min();
        return (min, max);
    }
}
