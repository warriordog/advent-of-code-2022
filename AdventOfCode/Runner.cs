using AdventOfCode.Common;
using AdventOfCode.Day01;
using AdventOfCode.Day02;
using AdventOfCode.Day03;
using Microsoft.Extensions.Options;

namespace AdventOfCode;

/// <summary>
/// Runs and introspects solutions
/// </summary>
public class Runner
{
    private static readonly Dictionary<string, Dictionary<string, Type>> SolutionMap = new()
    {
        { "Day01", new() {
            { "Part1", typeof(Day01Part1) },
            { "Part2", typeof(Day01Part2) }
        }},
        { "Day02", new() {
            { "Part1", typeof(Day02Part1) },
            { "Part2", typeof(Day02Part2) }
        }},
        { "Day03", new() {
            { "Part1", typeof(Day03Part1) },
            { "Part2", typeof(Day03Part2) }
        }}
    };

    private readonly FileInfo? _customInputFile;
    private readonly IServiceProvider _serviceProvider;

    public Runner(IOptions<RunnerOptions> runnerOptions, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _customInputFile = runnerOptions.Value.CustomInputFile;
    }

    public async Task ExecuteListCommand(string? day)
    {
        await Console.Out.WriteLineAsync($"Listing solutions for {day}.");

        if (day == null)
        {
            foreach (var dayKey in SolutionMap.Keys)
            {
                await ListSolutionsForDay(dayKey);
            }
        }
        else
        {
            await ListSolutionsForDay(day);
        }
    }

    private async Task ListSolutionsForDay(string dayKey)
    {
        await Console.Out.WriteLineAsync($"{dayKey}:");
        if (SolutionMap.TryGetValue(dayKey, out var partMap) && partMap.Any())
        {
            foreach (var (part, _) in partMap)
            {
                await Console.Out.WriteLineAsync($"    {part}");
            }
        }
        else
        {
            await Console.Out.WriteLineAsync("    * no results *");
        }
    }

    public async Task ExecuteRunCommand(string? day, string? part)
    {
        if (day == null)
        {
            foreach (var dayKey in SolutionMap.Keys)
            {
                await ExecuteDay(dayKey, part);
            }
        }
        else
        {
            await ExecuteDay(day, part);
        }
    }

    private async Task ExecuteDay(string dayKey, string? part)
    {
        if (!SolutionMap.TryGetValue(dayKey, out var partMap))
        {
            await Console.Error.WriteLineAsync($"No solutions found for {dayKey}");
            throw new ArgumentException($"Day is not in SolutionMap - {dayKey}", nameof(dayKey));
        }

        if (part == null)
        {
            foreach (var partKey in partMap.Keys)
            {
                await ExecuteDayPart(dayKey, partKey);
            }
        }
        else
        {
            await ExecuteDayPart(dayKey, part);
        }
    }

    private async Task ExecuteDayPart(string dayKey, string partKey)
    {
        // Verify and load input file
        var inputPath = _customInputFile?.FullName ?? $"AdventOfCode/{dayKey}/input.txt";
        if (!File.Exists(inputPath))
        {
            await Console.Error.WriteLineAsync($"Cannot find input file at \"{inputPath}\". Did you mistype it?");
            throw new ApplicationException($"Input file could not be found: \"{inputPath}\"");
        }
        var input = await File.ReadAllTextAsync(inputPath);

        // Execute day
        var solutionType = SolutionMap[dayKey][partKey];
        var solution = _serviceProvider.CreateInstance<ISolution>(solutionType);
        solution.Run(input);
    }
}

public class RunnerOptions
{
    public FileInfo? CustomInputFile { get; set; }
}