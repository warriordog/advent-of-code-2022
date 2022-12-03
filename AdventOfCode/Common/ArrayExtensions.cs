namespace AdventOfCode.Common;

public static class ArrayExtensions
{
    public static T[][] SplitArray<T>(this T[] array)
    {
        if (array.Length < 1) throw new ArgumentException("Cannot split an empty array", nameof(array));
        return SplitArray(array, array.Length / 2);
    }
    
    public static T[][] SplitArray<T>(this T[] array, int idx)
    {
        if (array.Length < 1) throw new ArgumentException("Cannot split an empty array", nameof(array));
        if (idx < 0) throw new ArgumentOutOfRangeException(nameof(idx), "Index must be greater or equal to zero");
        if (idx >= array.Length) throw new ArgumentOutOfRangeException(nameof(idx), "Index must be less than the length of array");
        
        
        // Create 1st array
        var firstLength = idx;
        var first = new T[firstLength];
        Array.Copy(array, 0, first, 0, firstLength);
        
        // create 2nd array
        var secondLength = array.Length - idx;
        var second = new T[secondLength];
        if (secondLength > 0)
        {
            Array.Copy(array, idx, second, 0, secondLength);
        }

        return new[] { first, second };
    }
}