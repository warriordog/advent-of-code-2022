using AdventOfCode.Common;
using AdventOfCode.Day01;
using AdventOfCode.Day02;
using AdventOfCode.Day03;

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
        }},
        { "Day03", new() {
            { "Part1", () => new Day03Part1() },
            { "Part2", () => new Day03Part2() }
        }}
    };

    public static async Task Main(string[] args)
    {
        var operation = args.Parse("operation").ToLower();
        if (operation == "all") await RunAllSolutions();
        else if (operation == "run") await RunOneSolution(args, false);
        else if (operation == "test") await RunOneSolution(args, true);
        else
        {
            await Console.Error.WriteLineAsync($"Unknown operation {operation}. Supported values are 'all', 'run', and 'test'.");
            throw new ApplicationException($"Unknown operation {operation}");
        }
    }

    private static async Task RunAllSolutions()
    {
        foreach (var (day, partMap) in SolutionMap)
        {
            foreach (var factory in partMap.Values)
            {
                await ExecuteSolution(day, factory, false);
            }
        }
    }

    private static async Task RunOneSolution(string[] args, bool isTest)
    {
        var (dayName, partName) = args
            .Skip(1)
            .ToList()
            .Parse("day name/number", "part name/number");
        
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
        
        await ExecuteSolution(dayName, factory, isTest);
    }

    private static async Task ExecuteSolution(string day, Func<ISolution> factory, bool isTest)
    {
        var inputFileName = isTest ? "test" : "input";
        var inputPath = $"AdventOfCode/{day}/{inputFileName}.txt";
        var inputFile = await File.ReadAllTextAsync(inputPath);

        if (isTest)
        {
            await Console.Out.WriteLineAsync("Starting in TEST mode.");
        }
        
        var solution = factory();
        solution.Run(inputFile);
    }
}