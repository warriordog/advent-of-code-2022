using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day05;

[Solution("Day05", "Part2")]
public class Day05Part2 : Day05<List<char>>
{
    public Day05Part2(ILogger<Day05Part2> logger) : base(logger) {}

    protected override void AddToStack(List<char> stack, char crate) => stack.Add(crate);
    protected override void ExecuteMove(List<char> from, List<char> to, byte count) => MoveLast(from, to, count);
    protected override IEnumerable<char> GetTopOfStacks(IEnumerable<List<char>> stacks) => stacks.Select(s => s.LastOrDefault(' '));

    /// <summary>
    /// Moves the last N elements from one list to another, preserving order.
    /// The elements will be removed from the source.
    /// </summary>
    /// <param name="source">Source list</param>
    /// <param name="dest">Destination list</param>
    /// <param name="count">Number of elements to move</param>
    /// <typeparam name="T">Type of element</typeparam>
    private static void MoveLast<T>(List<T> source, List<T> dest, int count)
    {
        if (count == 0) return;
        
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative");
        if (count > source.Count) throw new ArgumentOutOfRangeException(nameof(count), $"Insufficient elements in source - tried to take {count} elements but source only had {source.Count}.");

        var copyStart = source.Count - count;
        
        // Copy elements
        for (var i = copyStart; i < source.Count; i++)
        {
            var element = source[i];
            dest.Add(element);
        }
        
        // Remove from source.
        // This works backwards to hit the happy path of RemoveAt().
        // When the last element is removed, no shifting or resizing is performed.
        for (var i = source.Count - 1; i >= copyStart; i--)
        {
            source.RemoveAt(i);
        }
    }
}