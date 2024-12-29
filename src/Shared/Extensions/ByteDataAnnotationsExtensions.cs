using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BytePlatform.Shared.Extensions;
public static class ByteDataAnnotationsExtensions
{
    private const string REQUIRED_ATTRIBUTE_NAME = "System.ComponentModel.DataAnnotations.RequiredAttribute";
    private const string REDEMPTION_HAS_ANY_ITEM_ATTRIBUTE_NAME = "System.ComponentModel.DataAnnotations.RedemptionHasAnyItemAttribute";

    public static bool HasRequiredAttribute(this Expression expression)
    {
        return ((MemberExpression)expression).Member.CustomAttributes
                 .Any(e => REQUIRED_ATTRIBUTE_NAME.OrdinalEquals(e.AttributeType.FullName) ||
                           REQUIRED_ATTRIBUTE_NAME.OrdinalEquals(e.AttributeType.BaseType?.FullName) ||
                           REDEMPTION_HAS_ANY_ITEM_ATTRIBUTE_NAME.OrdinalEquals(e.AttributeType.FullName));
    }

    public static string? GetDisplayName(this Expression expression)
    {
        var member = ((MemberExpression)expression).Member;
        if (Attribute.GetCustomAttribute(member, typeof(DisplayAttribute)) is DisplayAttribute displayAttribute)
        {
            return displayAttribute.Name ?? member.Name;
        }

        return member.Name;
    }

    public static int? GetMaxLength(this Expression expression)
    {
        var member = ((MemberExpression)expression).Member;

        if (Attribute.GetCustomAttribute(member, typeof(StringLengthAttribute)) is StringLengthAttribute stringLengthAttribute)
        {
            return stringLengthAttribute.MaximumLength;
        }

        if (Attribute.GetCustomAttribute(member, typeof(MaxLengthAttribute)) is MaxLengthAttribute maxLengthAttribute)
        {
            return maxLengthAttribute.Length;
        }

        return null;
    }
}
