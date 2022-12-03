using AdventOfCode.Common;

namespace AdventOfCode.Day01;

public abstract class Day01 : ISolution
{
    public void Run(string inputFile)
    {
        var elves = inputFile
                .SplitByTwoThenOneEOL()
                .Select(chunk => chunk.Aggregate(0, (sum, line) => sum + int.Parse(line)))
            ;
        
        RunPart(elves);
    }

    protected abstract void RunPart(IEnumerable<int> calories);
    protected void Log(string message) => Console.WriteLine($"[{GetType().Name}] {message}");
}