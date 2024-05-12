namespace Astrum.Recruting.Application.Shared;

public static class AssertExtension
{
    public static void AssertFound<T>(this T element)
    {
        if (element is null)
            throw new ArgumentException("Запрашиваемый ресурс не найден");
    }
}