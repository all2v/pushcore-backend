namespace Astrum.SharedLib.Domain.Extensions;

public static class EnumerableExtensions
{
    public static int GetHashCode<T>(this IEnumerable<T> enumerable)
    {
        const int seed = 487;
        const int modifier = 31;

        return enumerable.Aggregate(seed, (current, item) =>
            current * modifier + item.GetHashCode());
    }
}