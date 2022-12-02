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
                .Select((calories, index) => new Elf(index + 1, calories))
            ;
        
        RunPart(elves);
    }

    protected abstract void RunPart(IEnumerable<Elf> elves);
    protected void Log(string message) => Console.WriteLine($"[{GetType().Name}] {message}");
}

public record Elf(int Number, int Calories);