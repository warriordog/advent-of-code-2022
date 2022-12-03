using System.CommandLine;
using AdventOfCode.Common;
using AdventOfCode.Day01;
using AdventOfCode.Day02;
using AdventOfCode.Day03;

namespace AdventOfCode;

public static class Program
{
    private const string AllDaysToken = "all";
    private const string AllPartsToken = "all";
    
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

    public static async Task<int> Main(string[] args)
    {
        var rootCmd = new RootCommand("Advent of Code Solution Runner");
        var runCmd = new Command("run", "Run a solution");
        var runInputOption = new Option<FileInfo>("--input", description: "File to use as problem input (defaults to input.txt in the solution folder)");
        var runDayArgument = new Argument<string>("day", $"day number (Day## or '{AllDaysToken}')");
        var runPartArgument = new Argument<string>("part", description: $"part number (Part# or '{AllPartsToken}')", getDefaultValue: () => AllPartsToken);
        var listCmd = new Command("list", "List known solutions");
        var listDayArgument = new Argument<string>("day", description: $"day number (Day## or '{AllDaysToken}')", getDefaultValue: () => AllDaysToken);

        runInputOption.AddAlias("-i");
        runCmd.AddOption(runInputOption);
        runCmd.AddArgument(runDayArgument);
        runCmd.AddArgument(runPartArgument);
        runCmd.SetHandler(ExecuteRunCommand, runInputOption, runDayArgument, runPartArgument);
        rootCmd.Add(runCmd);
        
        listCmd.AddArgument(listDayArgument);
        listCmd.SetHandler(ExecuteListCommand, listDayArgument);
        rootCmd.Add(listCmd);
        
        
        return await rootCmd.InvokeAsync(args);
    }
    
    private static void ExecuteListCommand(string day)
    {
        Console.WriteLine($"Listing solutions for {day}.");
        
        if (day.Equals(AllDaysToken, StringComparison.OrdinalIgnoreCase))
        {
            foreach (var dayKey in SolutionMap.Keys)
            {
                ListSolutionsForDay(dayKey);
            }
        }
        else
        {
            ListSolutionsForDay(day);
        }
    }
    
    private static void ListSolutionsForDay(string dayKey)
    {
        Console.WriteLine($"{dayKey}:");
        if (SolutionMap.TryGetValue(dayKey, out var partMap) && partMap.Any())
        {
            foreach (var (part, _) in partMap)
            {
                Console.WriteLine($"    {part}");
            }
        }
        else
        {
            Console.WriteLine("    * no results *");
        }
    }

    private static async Task ExecuteRunCommand(FileInfo? inputFile, string day, string part)
    {
        if (day.Equals(AllDaysToken, StringComparison.OrdinalIgnoreCase))
        {
            foreach (var dayKey in SolutionMap.Keys)
            {
                await ExecuteDay(inputFile, dayKey, part);
            }
        }
        else
        {
            await ExecuteDay(inputFile, day, part);
        }
    }
    
    private static async Task ExecuteDay(FileInfo? inputFile, string dayKey, string part)
    {
        if (!SolutionMap.TryGetValue(dayKey, out var partMap))
        {
            await Console.Error.WriteLineAsync($"No solutions found for {dayKey}");
            throw new ArgumentException($"Day is not in SolutionMap - {dayKey}", nameof(dayKey));
        }
        
        if (part.Equals(AllPartsToken, StringComparison.OrdinalIgnoreCase))
        {
            foreach (var partKey in partMap.Keys)
            {
                await ExecuteDayPart(inputFile, dayKey, partKey);
            } 
        }
        else
        {
            await ExecuteDayPart(inputFile, dayKey, part);
        }
    }
    
    private static async Task ExecuteDayPart(FileInfo? inputFile, string dayKey, string partKey)
    {
        // Verify and load input file
        var inputPath = inputFile?.FullName ?? $"AdventOfCode/{dayKey}/input.txt";
        if (!File.Exists(inputPath))
        {
            await Console.Error.WriteLineAsync($"Cannot find input file at \"{inputPath}\". Did you mistype it?");
            throw new ArgumentException("Input file could not be found", nameof(inputFile));
        }
        var input = await File.ReadAllTextAsync(inputPath);

        // Execute day
        var factory = SolutionMap[dayKey][partKey];
        var solution = factory();
        solution.Run(input);
    }
}