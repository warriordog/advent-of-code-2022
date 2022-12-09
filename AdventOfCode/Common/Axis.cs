namespace AdventOfCode.Common;

public class Axis<T>
{
    public int Min { get; private set; }
    public int Max { get; private set; }
    
    public T? this[int idx]
    {
        get => Get(idx);
        set => Set(idx, value);
    }
    
    private readonly List<T?> _negativeAxis = new();
    private readonly List<T?> _positiveAxis = new();
    
    public void Set(int index, T? value)
    {
        if (index >= 0)
            SetIn(_positiveAxis, index, value);
        else
            SetIn(_negativeAxis, (0 - index) - 1, value);

        if (index > Max)
            Max = index;
        if (index < Min)
            Min = index;
    }

    private static void SetIn(List<T?> list, int index, T? value)
    {
        // Pad the list if needed
        while (index > list.Count)
        {
            list.Add(default);
        }

        if (index == list.Count)
        {
            // Add the item to the end
            list.Add(value);
        }
        else
        {
            // Set the item in the list
            list[index] = value;
        }
    }

    public T? Get(int index)
    {
        if (index >= 0)
            return GetIn(_positiveAxis, index);
        else
            return GetIn(_negativeAxis, (0 - index) - 1);
    }
    
    private T? GetIn(List<T?> list, int index)
    {
        // Index may be out of bounds - need to handle it
        if (index >= list.Count)
        {
            return default;
        }
        
        // Index is in bounds
        return list[index];
    }
    
    public void Clear()
    {
        _positiveAxis.Clear();
        _negativeAxis.Clear();
    }
}
//
// internal readonly record struct AxisEntry<T>
// {
//     public T? Value { get; }
//
//     public bool HasValue
//     {
//         [MemberNotNullWhen(true, nameof(Value))]
//         get;
//     }
//     
//     public AxisEntry(T value)
//     {
//         Value = value;
//         HasValue = true;
//     }
//     public AxisEntry()
//     {
//         Value = default;
//         HasValue = false;
//     }
//     
//     public void Deconstruct(out T? value, out bool hasValue)
//     {
//         value = Value;
//         hasValue = HasValue;
//     }
//
//     public bool Equals(AxisEntry<T> other) => EqualityComparer<T?>.Default.Equals(Value, other.Value) && HasValue == other.HasValue;
//     public override int GetHashCode() => HashCode.Combine(Value, HasValue);
//
//
//     private sealed class ValueHasValueEqualityComparer : IEqualityComparer<AxisEntry<T>>
//     {
//         public bool Equals(AxisEntry<T> x, AxisEntry<T> y)
//         {
//             return EqualityComparer<T?>.Default.Equals(x.Value, y.Value) && x.HasValue == y.HasValue;
//         }
//         public int GetHashCode(AxisEntry<T> obj)
//         {
//             return HashCode.Combine(obj.Value, obj.HasValue);
//         }
//     }
//
//     public static IEqualityComparer<AxisEntry<T>> ContentEqualityComparer { get; } = new ValueHasValueEqualityComparer();
// }