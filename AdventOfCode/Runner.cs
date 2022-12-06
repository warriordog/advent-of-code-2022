using AdventOfCode.Benchmark;
using AdventOfCode.Common;
using AdventOfCode.Day01;
using AdventOfCode.Day02;
using AdventOfCode.Day03;
using AdventOfCode.Day04;
using AdventOfCode.Day05;
using AdventOfCode.Day06;
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
        }},
        { "Day04", new() {
            { "Part1", typeof(Day04Part1) },
            { "Part2", typeof(Day04Part2) }
        }},
        { "Day05", new() {
            { "Part1", typeof(Day05Part1) },
            { "Part2", typeof(Day05Part2) }
        }},
        { "Day06", new() {
            { "Part1", typeof(Day06Part1) },
            { "Part2", typeof(Day06Part2) },
            { "Part2SmallList", typeof(Day06Part2SmallList) },
            { "Part2BitFields", typeof(Day06Part2BitFields) }
        }}
    };

    private readonly FileInfo? _customInputFile;
    private readonly BenchmarkRunner _benchmarkRunner;
    private readonly IServiceProvider _serviceProvider;

    public Runner(IOptions<RunnerOptions> runnerOptions, IServiceProvider serviceProvider, BenchmarkRunner benchmarkRunner)
    {
        _serviceProvider = serviceProvider;
        _benchmarkRunner = benchmarkRunner;
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
        await ExecuteSolutions(day, part, (solution, _, input) =>
        {
            solution.Run(input);
            return Task.CompletedTask;
        });
    }

    public async Task ExecuteBenchCommand(string? day, string? part)
    {
        await ExecuteSolutions(day, part, async (solution, def, input) =>
            {
                var results = _benchmarkRunner.ExecuteBenchmark(solution, input);
                await Console.Out.WriteLineAsync($"Benchmark of {def.Day} {def.Part} completed:");
                if (results.WarmupWasRun)
                    await Console.Out.WriteLineAsync($"    Warmup took {results.TotalWarmupTimeMs}ms to complete {results.TotalWarmupRounds} rounds.");
                else
                    await Console.Out.WriteLineAsync("    Warmup was disabled and skipped.");
                await Console.Out.WriteLineAsync($"    Sample took {results.TotalSampleTimeMs}ms to complete {results.TotalSampleRounds} rounds.");
                await Console.Out.WriteLineAsync($"    This solution completes in an average of {results.AverageTimeMs}ms per run.");
            }
        );
    }

    private async Task ExecuteSolutions(string? day, string? part, Func<ISolution, SolutionDef, string, Task> executor)
    {
        var solutions = ResolveSolutions(day, part);
        foreach (var resolvedSolution in solutions)
        {
            await ExecuteSolution(resolvedSolution, executor);
        }
    }
    
    private List<SolutionDef> ResolveSolutions(string? day, string? part)
    {
        var solutions = new List<SolutionDef>();
        if (day == null)
        {
            foreach (var (dayKey, partMap) in SolutionMap)
            {
                ResolveSolutionDay(partMap, dayKey, part, solutions);
            }
        }
        else
        {
            if (!SolutionMap.TryGetValue(day, out var partMap))
            {
                Console.Error.WriteLine($"No solutions found for {day}");
                throw new ArgumentException($"Day is not in SolutionMap - {day}", nameof(day));
            }
            
            ResolveSolutionDay(partMap, day, part, solutions);
        }
        return solutions;
    }

    private void ResolveSolutionDay(Dictionary<string, Type> partMap, string day, string? part, List<SolutionDef> solutions)
    {
        if (part == null)
        {
            foreach (var (partKey, type) in partMap)
            {
                var solution = new SolutionDef(type, day, partKey);
                solutions.Add(solution);
            }
        }
        else
        {
            if (!partMap.TryGetValue(part, out var solutionType))
            {
                Console.Error.WriteLine($"{day} does not have a part called {part}.");
                throw new ArgumentException($"Part is not in partMap: {part}", nameof(partMap));
            }

            var solution = new SolutionDef(solutionType, day, part);
            solutions.Add(solution);
        }
    }

    private async Task ExecuteSolution(SolutionDef solutionDef, Func<ISolution, SolutionDef, string, Task> executor)
    {
        var input = await LoadInputFile(solutionDef.Day);
        var solution = _serviceProvider.CreateInstance<ISolution>(solutionDef.SolutionType);
        await executor(solution, solutionDef, input);
    }
    
    private async Task<string> LoadInputFile(string dayKey)
    {
        var inputPath = _customInputFile?.FullName ?? $"AdventOfCode/{dayKey}/input.txt";
        if (!File.Exists(inputPath))
        {
            await Console.Error.WriteLineAsync($"Cannot find input file at \"{inputPath}\". Did you mistype it?");
            throw new ApplicationException($"Input file could not be found: \"{inputPath}\"");
        }

        var input = await File.ReadAllTextAsync(inputPath);
        return input;
    }
}

internal record SolutionDef(Type SolutionType, string Day, string Part); 

public class RunnerOptions
{
    public FileInfo? CustomInputFile { get; set; }
}