using AdventOfCode.Common;

namespace AdventOfCode.Day01;

public abstract class Day01 : ISolution
{
    public async Task Run(string inputFile, List<string> solutionArgs)
    {
        var inputText = await File.ReadAllTextAsync(inputFile);
        var elves = inputText
                .SplitByTwoThenOneEOL()
                .Select(chunk => chunk.Aggregate(0, (sum, line) => sum + int.Parse(line)))
            ;
        
        RunPart(elves);
    }

    protected abstract void RunPart(IEnumerable<int> calories);
    protected void Log(string message) => Console.WriteLine($"[{GetType().Name}] {message}");
}