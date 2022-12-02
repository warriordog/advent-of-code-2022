using AdventOfCode.Common;

namespace AdventOfCode.Day01;

public static class Day01
{
    public static async Task<List<Elf>> ParseInput()
    {
        var inputPath = "AdventOfCode/Day01/day1.txt";
        var inputText = await File.ReadAllTextAsync(inputPath);
        var elves = inputText
                .SplitByTwoThenOneEOL()
                .Select(chunk => chunk.Aggregate(0, (sum, line) => sum + int.Parse(line)))
                .Select((calories, index) => new Elf(index + 1, calories))
                .ToList()
            ;
        return elves;
    }
}

public record Elf(int Number, int Calories);