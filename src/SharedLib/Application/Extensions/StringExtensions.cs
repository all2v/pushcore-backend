using System.Text;
using System.Text.RegularExpressions;

namespace Astrum.SharedLib.Application.Extensions;

public static class StringExtensions
{
    public static string GenerateSlug(this string phrase)
    {
        var str = phrase.ToLower().Trim();
        // invalid chars           
        str = Regex.Replace(str, @"[^a-zа-я0-9\s-]", "");
        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim();
        // trim 
        if (str.Length > 45)
            str = str.Substring(0, 45);
        str = Regex.Replace(str, @"\s", "-"); // hyphens   
        return str;
    }

    public static string RemoveAccent(this string txt)
    {
        var bytes = Encoding.UTF8.GetBytes(txt);
        return Encoding.ASCII.GetString(bytes);
    }

    public static string FilterSpecialChars(this string str)
    {
        var result = String.Copy(str);
        result
            .Where(x => !char.IsLetter(x))
            .ToList()
            .ForEach(c => result = result.Replace(c.ToString(), string.Empty));
        return result;
    }
}