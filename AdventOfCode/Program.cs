using AdventOfCode.Common;
using AdventOfCode.Day01;
using AdventOfCode.Day02;

namespace AdventOfCode;

public static class Program
{
    private static readonly Dictionary<string, Dictionary<string, Func<ISolution>>> SolutionMap = new()
    {
        { "Day01", new() {
            { "Part1", () => new Day01Part1() },
            { "Part2", () => new Day01Part2() }
        }},
        { "Day02", new() {
            { "Part1", () => new Day02Part1() },
            { "Part2", () => new Day02Part2() }
        }}
    };

    public static async Task Main(string[] args)
    {
        if (args.Length == 1 && args[0].ToLower() == "all")
        {
            await RunAllSolutions();
        }
        else
        {
            await RunOneSolution(args);
        }
    }

    private static async Task RunAllSolutions()
    {
        foreach (var (day, partMap) in SolutionMap)
        {
            foreach (var factory in partMap.Values)
            {
                await ExecuteSolution(day, factory, new List<string>());
            }
        }
    }

    private static async Task RunOneSolution(string[] args)
    {
        var (dayName, partName) = args.Parse("day name/number", "part name/number");
        
        if (!SolutionMap.TryGetValue(dayName, out var partMap))
        {
            await Console.Error.WriteLineAsync($"Unknown day \"{dayName}\"");
            throw new ApplicationException("Day was not found in SolutionMap");
        }
        if (!partMap.TryGetValue(partName, out var factory))
        {
            await Console.Error.WriteLineAsync($"Unknown part \"{partName}\" for day \"{dayName}\"");
            throw new ApplicationException("Part was not found in SolutionMap");
        }
        
        var solutionArgs = args.Skip(2).ToList();
        await ExecuteSolution(dayName, factory, solutionArgs);
    }

    private static async Task ExecuteSolution(string day, Func<ISolution> factory, List<string> args)
    {
        var inputFile = $"AdventOfCode/{day}/input.txt";
        var solution = factory();
        await solution.Run(inputFile, args);
    }
}