using AdventOfCode;
using Main.Benchmark;
using Main.Util;
using Microsoft.Extensions.Options;

namespace Main.Services;


/// <summary>
/// Runs and introspects solutions
/// </summary>
public interface ISolutionRunner
{
    /// <summary>
    /// Lists all solutions for a given day
    /// </summary>
    /// <param name="day">Optional day to filter by</param>
    /// <param name="part">Optional part to filer by</param>
    Task ExecuteListCommand(string? day, string? part);

    /// <summary>
    /// Runs one or more solutions
    /// </summary>
    /// <param name="day">Optional day to filter by</param>
    /// <param name="part">Optional part to filter by</param>
    /// <param name="variant">Optional variation to filter by</param>
    Task ExecuteRunCommand(string? day, string? part, string? variant);
    
    /// <summary>
    /// Benchmarks one or more solutions
    /// </summary>
    /// <param name="day">Optional day to filter by</param>
    /// <param name="part">Optional part to filter by</param>
    /// <param name="variant">Optional variation to filter by</param>
    Task ExecuteBenchCommand(string? day, string? part, string? variant);
}

public class SolutionRunner : ISolutionRunner
{
    private readonly FileInfo? _customInputFile;
    private readonly IBenchmarkRunner _benchmarkRunner;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISolutionRegistry _solutionRegistry;

    public SolutionRunner(IOptions<SolutionRunnerOptions> runnerOptions, IServiceProvider serviceProvider, IBenchmarkRunner benchmarkRunner, ISolutionRegistry solutionRegistry)
    {
        _serviceProvider = serviceProvider;
        _benchmarkRunner = benchmarkRunner;
        _solutionRegistry = solutionRegistry;
        _customInputFile = runnerOptions.Value.CustomInputFile;
    }

    // This is horrendous
    public async Task ExecuteListCommand(string? day, string? part)
    {
        var days = _solutionRegistry
            .GetDaysByFilter(day)
            .ToList();
        if (!days.Any())
        {
            await Console.Out.WriteLineAsync("* no results *");
            return;
        }

        foreach (var dayEntry in days)
        {
            await Console.Out.WriteLineAsync($"{dayEntry.Name}:");
            var parts = dayEntry
                .GetPartsByFilter(part)
                .ToList();
            if (!parts.Any())
            {
                await Console.Out.WriteLineAsync("    * no results *");
                continue;
            }

            foreach (var partEntry in parts)
            {
                var variants = partEntry.Solutions
                    .Where(s => s.IsVariant)
                    .ToList();

                var hasPartSolution = partEntry.Solutions.Any(s => !s.IsVariant);
                if (hasPartSolution)
                {
                    await Console.Out.WriteLineAsync($"    {partEntry.Name}");
                    foreach (var variant in variants)
                    {
                        await Console.Out.WriteLineAsync($"        + {variant.Variant}");
                    }
                }
                else
                {
                    await Console.Out.WriteLineAsync($"    {partEntry.Name} (variants only):");
                    foreach (var variant in variants)
                    {
                        await Console.Out.WriteLineAsync($"        {variant.Variant}");
                    }
                }
            }
        }
    }

    public async Task ExecuteRunCommand(string? day, string? part, string? variant) => await ExecuteSolutions(day, part, variant);

    public async Task ExecuteBenchCommand(string? day, string? part, string? variant) => await ExecuteSolutions(day, part, variant, true);

    private async Task ExecuteSolutions(string? day, string? part, string? variant, bool isBenchmark = false)
    {
        var solutions = _solutionRegistry
            .GetSolutionsByFilter(day, part, variant)
            .ToList();

        if (!solutions.Any())
        {
            Console.WriteLine("No solutions found matching those criteria.");
            return;
        }
        
        foreach (var solutionEntry in solutions)
        {
            var input = await LoadInputFile(solutionEntry.Day);
            var solution = _serviceProvider.CreateInstance<ISolution>(solutionEntry.SolutionType);

            if (isBenchmark)
            {
                await ExecuteSolutionBenchmark(solution, solutionEntry, input);
            }
            else
            {
                solution.Run(input);
            }
        }
    }
    private async Task ExecuteSolutionBenchmark(ISolution solution, SolutionEntry entry, string input)
    {
        var results = _benchmarkRunner.ExecuteBenchmark(solution, input);
        var name = entry.IsVariant ? $"{entry.Day} {entry.Part} {entry.Variant}" : $"{entry.Day} {entry.Part}";
        await Console.Out.WriteLineAsync($"Benchmark of {name} completed:");
        if (results.WarmupWasRun)
            await Console.Out.WriteLineAsync($"    Warmup took {results.TotalWarmupTimeMs}ms to complete {results.TotalWarmupRounds} rounds.");
        else
            await Console.Out.WriteLineAsync("    Warmup was disabled and skipped.");
        await Console.Out.WriteLineAsync($"    Sample took {results.TotalSampleTimeMs}ms to complete {results.TotalSampleRounds} rounds.");
        await Console.Out.WriteLineAsync($"    This solution completes in an average of {results.AverageTimeMs}ms per run.");
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

public class SolutionRunnerOptions
{
    public FileInfo? CustomInputFile { get; set; }
}