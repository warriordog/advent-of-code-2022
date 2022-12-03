using AdventOfCode.Common;

namespace AdventOfCode.Day03;

public abstract class Day03 : ISolution
{
    public void Run(string inputFile)
    {
        var groups = inputFile
            .SplitByEOL()
            .SkipEmptyStrings()
            .Chunk(3)
            .Select(group => group
                .Select(line => line
                    .ToCharArray()
                    .SplitArray()
                    .Select(PivotItemPriorities)
                    .ToArray()
                )
                .ToArray()
            );
        
        RunDay3(groups);
    }

    private int[] PivotItemPriorities(IEnumerable<char> items)
    {
        var priorityCounts = new int[53];
        foreach (var item in items)
        {
            int priority;
            if (item >= 'a') priority = 1 + (item - 'a');
            else priority = 27 + (item - 'A');

            // Do we like bound checks? Yes we do.
            if (priority is < 1 or > 52)
            {
                throw new ArgumentException($"Input character did not map to a valid priority. It should be within a-z or A-Z. Character was: '{item}'", nameof(items));
            }
            
            priorityCounts[priority]++;
        }
        return priorityCounts;
    }

    protected abstract void RunDay3(IEnumerable<int[][][]> groups);

    
    protected void Log(string message) => Console.WriteLine($"[{GetType().Name}] {message}");
    protected static IEnumerable<int> AllPriorities => Enumerable.Range(1, 52);
}