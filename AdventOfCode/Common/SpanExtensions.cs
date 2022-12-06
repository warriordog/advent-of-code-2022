namespace AdventOfCode.Common;

public static class SpanExtensions
{
    public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex = 0)
    {
        if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex), "startIndex must be at least zero");
        if (startIndex >= span.Length) throw new ArgumentOutOfRangeException(nameof(startIndex), "startIndex must be less than span length");

        for (var i = startIndex; i < span.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(value, span[i]))
            {
                return i;
            } 
        }

        return -1;
    }
}