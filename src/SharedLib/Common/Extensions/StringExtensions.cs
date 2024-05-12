using System.Text;
using System.Text.RegularExpressions;

namespace Astrum.SharedLib.Common.Extensions;

public static class StringExtensions
{
    public static T ToEnum<T>(this string value) where T : struct
    {
        if (!Enum.TryParse<T>(value, out var enumeration)) return default;
        return enumeration;
    }
    public static string PascalToKebabCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])",
                "-$1",
                RegexOptions.Compiled)
            .Trim()
            .ToLower();
    }

    public static string ToInitials(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;

        var builder = new StringBuilder();

        var words = value.Split(" ");
        foreach (var word in words) builder.Append(word.Substring(0, 1));

        return builder.ToString().ToUpper();
    }

    public static string TrimStart(this string target, string trimString)
    {
        if (string.IsNullOrEmpty(trimString)) return target;

        var result = target;
        while (result.StartsWith(trimString)) result = result.Substring(trimString.Length);

        return result;
    }

    public static string ReverseSubstring(this string text, int startIndex, int endIndex)
    {
        var result = text[..startIndex];
        result += text.Substring(endIndex, text.Length - 1 - endIndex);
        return result;
    }

    public static string ReverseSubstring(this string text, string startTag, string endTag)
    {
        var startIndex = text.IndexOf(startTag, StringComparison.Ordinal);
        var endIndex = text.IndexOf(endTag, StringComparison.Ordinal) + endTag.Length;
        return startIndex != -1 ? text.ReverseSubstring(startIndex, endIndex) : text;
    }
}