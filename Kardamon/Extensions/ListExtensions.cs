namespace Kardamon.Extensions;

public static class ListExtensions
{
    public static List<T> Shuffle<T>(this IEnumerable<T> list)
    {
        var rnd = new Random();
        var result = new List<T>(list);

        for (int i = result.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            (result[i], result[j]) = (result[j], result[i]);
        }

        return result;
    }
}