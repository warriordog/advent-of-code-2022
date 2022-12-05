using System.CommandLine;
using AdventOfCode;
using AdventOfCode.Benchmark;
using AdventOfCode.Common;
using AdventOfCode.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

const string AllDaysToken = "all";
const string AllPartsToken = "all";

var rootCmd = new RootCommand("Advent of Code Solution Runner");
var runCmd = new Command("run", "Run a solution");
var inputOption = new Option<FileInfo>("--input", description: "File to use as problem input (defaults to input.txt in the solution folder)");
var requiredDayArgument = new Argument<string>("day", $"day number (Day## or '{AllDaysToken}')");
var partArgument = new Argument<string>("part", description: $"part number (Part# or '{AllPartsToken}')", getDefaultValue: () => AllPartsToken);
var listCmd = new Command("list", "List known solutions");
var optionalDayArgument = new Argument<string>("day", description: $"day number (Day## or '{AllDaysToken}')", getDefaultValue: () => AllDaysToken);
var benchCmd = new Command("bench", "Benchmark a solution");
var warmupTimeOption = new Option<double>("--min-warmup-time", description: "Minimum time (in milliseconds) to run warmup rounds", getDefaultValue: () => 2000.0d);
var warmupRoundsOption = new Option<int>("--min-warmup-rounds", description: "Minimum number of warmup rounds", getDefaultValue: () => 10);
var sampleTimeOption = new Option<double>("--min-sample-time", description: "Minimum time (in milliseconds) to run sampling (benchmark) rounds", getDefaultValue: () => 10000.0d);
var sampleRoundsOption = new Option<int>("--min-sample-rounds", description: "Minimum number of sample (benchmark) rounds", getDefaultValue: () => 10);
var disableWarmupOption = new Option<bool>("--disable-warmup", description: "Disables the warmup rounds while benchmarking", getDefaultValue: () => false);

inputOption.AddAlias("-i");
runCmd.AddOption(inputOption);
runCmd.AddArgument(requiredDayArgument);
runCmd.AddArgument(partArgument);
runCmd.SetHandler(ExecuteRunCommand, inputOption, requiredDayArgument, partArgument);
rootCmd.Add(runCmd);

listCmd.AddArgument(optionalDayArgument);
listCmd.SetHandler(ExecuteListCommand, optionalDayArgument);
rootCmd.Add(listCmd);

benchCmd.AddAlias("benchmark");
benchCmd.AddOption(inputOption);
benchCmd.AddOption(warmupTimeOption);
benchCmd.AddOption(warmupRoundsOption);
benchCmd.AddOption(sampleTimeOption);
benchCmd.AddOption(sampleRoundsOption);
benchCmd.AddOption(disableWarmupOption);
benchCmd.AddArgument(requiredDayArgument);
benchCmd.AddArgument(partArgument);
benchCmd.SetHandler(ExecuteBenchCommand, inputOption, requiredDayArgument, partArgument, warmupTimeOption, warmupRoundsOption, sampleTimeOption, sampleRoundsOption, disableWarmupOption);
rootCmd.Add(benchCmd);

// Parse command line
return await rootCmd.InvokeAsync(args);

// Adapter for list command
async Task ExecuteListCommand(string day)
{
    var dayForRunner = ConvertIdentifier(day);
    
    var runner = Setup();
    await runner.ExecuteListCommand(dayForRunner);
}

// Adapter for run command
async Task ExecuteRunCommand(FileInfo? inputFile, string day, string part)
{
    var dayForRunner = ConvertIdentifier(day);
    var partForRunner = ConvertIdentifier(part);

    var runner = SetupRunVariant(inputFile);
    await runner.ExecuteRunCommand(dayForRunner, partForRunner);
}

// Adapter for bench command
async Task ExecuteBenchCommand(FileInfo? inputFile, string day, string part, double warmupTime, int warmupRounds, double sampleTime, int sampleRounds, bool disableWarmup)
{
    var dayForRunner = ConvertIdentifier(day);
    var partForRunner = ConvertIdentifier(part);

    var runner = SetupRunVariant(inputFile, builder => builder
        .ConfigureLogging(logging => logging
            .SetMinimumLevel(LogLevel.Warning)
        )
        .ConfigureServices(services => services
            .AddOptions<BenchmarkRunnerOptions>()
            .Configure(o =>
            {
                o.DisableWarmup = disableWarmup;
                o.MinWarmupTimeMs = warmupTime;
                o.MinWarmupRounds = warmupRounds;
                o.MinSampleTimeMs = sampleTime;
                o.MinSampleRounds = sampleRounds;
            })
        )
    );
    await runner.ExecuteBenchCommand(dayForRunner, partForRunner);
}

// Replace "all" with null
string? ConvertIdentifier(string id) => id.Equals(AllDaysToken, StringComparison.OrdinalIgnoreCase) ? null : id;

// Setup with run options applied
Runner SetupRunVariant(FileInfo? inputFile, Action<IHostBuilder>? adapter = null) => 
    Setup(builder =>
    {
        adapter?.Invoke(builder);
        builder.ConfigureServices(services => services
            .AddOptions<RunnerOptions>()
            .Configure(o => o.CustomInputFile = inputFile)
        );
    });

// Generic setup method
Runner Setup(Action<IHostBuilder>? adapter = null)
{
    var builder = Host.CreateDefaultBuilder();

    // Set config first so that adapter can override
    builder.ConfigureLogging(logging => logging
        .ClearProviders()
        .AddConsoleFormatter<SolutionConsoleFormatter, ConsoleFormatterOptions>()
        .AddConsole(console => console.FormatterName = nameof(SolutionConsoleFormatter))
    );
    
    // Let caller override config and pre-populate services
    adapter?.Invoke(builder);
    
    // Populate defaults
    builder.ConfigureServices(services =>
    {
        services.TryAddSingleton<Runner>();
        services.TryAddOptions<RunnerOptions>();
        services.TryAddTransient<BenchmarkRunner>();
        services.TryAddOptions<BenchmarkRunnerOptions>();
    });

    var host = builder.Build();
    return host.Services.GetRequiredService<Runner>();
}