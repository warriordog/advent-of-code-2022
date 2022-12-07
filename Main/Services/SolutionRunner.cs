using System.Text;
using AdventOfCode;
using Main.Benchmark;
using Main.Util;

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
    Task ExecuteListSolutionsCommand(string? day, string? part);

    /// <summary>
    /// Lists all solutions for a given day
    /// </summary>
    /// <param name="day">Optional day to filter by</param>
    /// <param name="part">Optional part to filter by</param>
    /// <param name="variant">Optional variation to filter by</param>
    Task ExecuteListInputsCommand(string? day, string? part, string? variant);

    /// <summary>
    /// Runs one or more solutions
    /// </summary>
    /// <remarks>
    /// <see cref="input"/> and <see cref="customInput"/> are resolved to a registered <see cref="IInputFile"/> by the <see cref="InputResolver"/> service.
    /// </remarks>
    /// <param name="day">Optional day to filter by</param>
    /// <param name="part">Optional part to filter by</param>
    /// <param name="variant">Optional variation to filter by</param>
    /// <param name="input">Optional input selection</param>
    /// <param name="customInput">Optional custom input path</param>
    Task ExecuteRunCommand(string? day, string? part, string? variant, string? input = null, string? customInput = null);
    
    /// <summary>
    /// Benchmarks one or more solutions
    /// </summary>
    /// <remarks>
    /// <see cref="input"/> and <see cref="customInput"/> are resolved to a registered <see cref="IInputFile"/> by the <see cref="InputResolver"/> service.
    /// </remarks>
    /// <param name="day">Optional day to filter by</param>
    /// <param name="part">Optional part to filter by</param>
    /// <param name="variant">Optional variation to filter by</param>
    /// <param name="input">Optional input selection</param>
    /// <param name="customInput">Optional custom input path</param>
    Task ExecuteBenchCommand(string? day, string? part, string? variant, string? input = null, string? customInput = null);
}

public class SolutionRunner : ISolutionRunner
{
    private readonly IBenchmarkRunner _benchmarkRunner;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISolutionRegistry _solutionRegistry;
    private readonly IInputResolver _inputResolver;

    public SolutionRunner(IServiceProvider serviceProvider, IBenchmarkRunner benchmarkRunner, ISolutionRegistry solutionRegistry, IInputResolver inputResolver)
    {
        _serviceProvider = serviceProvider;
        _benchmarkRunner = benchmarkRunner;
        _solutionRegistry = solutionRegistry;
        _inputResolver = inputResolver;
    }

    // This is horrendous
    public async Task ExecuteListSolutionsCommand(string? day, string? part)
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

    // The output is beautiful but no one will ever be able to read this.
    // By tomorrow, "no one" will include me as well.
    public async Task ExecuteListInputsCommand(string? day, string? part, string? variant)
    {
        var solutions = _solutionRegistry
            .GetSolutionsByFilter(day, part, variant)
            .ToList();
        if (!solutions.Any())
        {
            await Console.Out.WriteLineAsync("* no results *");
            return;
        }
        
        // Load allll the inputs for meta purposes
        var allInputs = solutions
            .Select(sol => (
                Solution: sol,
                Inputs: _inputResolver.GetRegisteredInputsForSolution(sol.SolutionType)))
            .ToList();

        // A bunch of little hacks
        var widthOfName = allInputs.SelectMany(e => e.Inputs).Max(i => i.Name?.Length ?? 0);
        var widthOfPath = allInputs.SelectMany(e => e.Inputs).Max(i => i.Path.Length);
        var widthOfType = Enum.GetNames<InputFileType>().Select(name => name.Length).Max();
        
        // Print a section for each solution
        foreach (var solution in solutions)
        {
            var solutionName = solution.IsVariant ? $"{solution.Day} {solution.Part} {solution.Variant}" : $"{solution.Day} {solution.Part}";
            await Console.Out.WriteLineAsync($"{solutionName}:");

            var inputsForSolution = allInputs
                .Where(e => e.Solution == solution)
                .SelectMany(e => e.Inputs)
                .ToList();
            if (!inputsForSolution.Any())
            {
                await Console.Out.WriteLineAsync("    * no results *");
                continue;
            }
            
            // Extra solution-specific hack
            var widthOfIndex = inputsForSolution.Count.ToString().Length;

            // Print each type as a separate subsection
            foreach (var type in Enum.GetValues<InputFileType>())
            {
                var inputsForType = inputsForSolution
                    .Where(i => i.Type == type)
                    .ToList();
                if (!inputsForType.Any())
                    continue;
                
                // Write each input in order.
                for (var index = 0; index < inputsForType.Count; index++)
                {
                    var input = inputsForType[index];
                    
                    var sb = new StringBuilder();
                    sb.Append("    ");

                    // Write type header
                    var typeLabel = (index == 0 ? $"{type}:" : "").PadRight(widthOfType + 1); // extra space for ":"
                    sb.Append(typeLabel);
                    
                    // Path to input.
                    // Left padding looks better in most cases because of file extensions.
                    sb.Append(' ');
                    sb.Append(input.Path.PadLeft(widthOfPath));
                    
                    if (widthOfName > 0) // Hack to include name if *any* has a name, for alignment
                    {
                        sb.Append(' ');
                        var inputName = input.Name != null ? $"\"{input.Name}\"" : "";
                        sb.Append(inputName.PadRight(widthOfName + 2)); // Name of the input + room for quotes
                    }
                    
                    if (input.Description != null)
                    {
                        sb.Append(" - ");
                        sb.Append(input.Description); // Description of input
                    }
                    
                    // Metadata & ID number
                    sb.Append(" [id=");
                    sb.Append(index.ToString().PadRight(widthOfIndex)); // index / ID
                    if (input.IsDefault)
                        sb.Append(", default");
                    if (input.Resolution.IsExternal())
                        sb.Append(", external");
                    if (input.Resolution.IsRelativeToCWD())
                        sb.Append(", relative");
                    sb.Append(']');

                    await Console.Out.WriteLineAsync(sb.ToString());
                }
            }
        }
    }

    public async Task ExecuteRunCommand(string? day, string? part, string? variant, string? input = null, string? customInput = null) => await ExecuteSolutions(day, part, variant, input, customInput);

    public async Task ExecuteBenchCommand(string? day, string? part, string? variant, string? input = null, string? customInput = null) => await ExecuteSolutions(day, part, variant, input, customInput, true);

    private async Task ExecuteSolutions(string? day, string? part, string? variant, string? selectedInput, string? customInput, bool isBenchmark = false)
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
            var input = await _inputResolver.LoadInputByUserSelection(solutionEntry, selectedInput, customInput);
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
}