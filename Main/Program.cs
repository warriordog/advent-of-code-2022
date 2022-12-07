using System.CommandLine;
using System.CommandLine.Invocation;
using AdventOfCode;
using Main.Benchmark;
using Main.Services;
using Main.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

const string AllDaysToken = "all";
const string AllPartsToken = "all";
const string AllVariantsToken = "all";

var rootCmd = new RootCommand("Advent of Code Solution Runner");
var runCmd = new Command("run", "Run a solution");
var customInputOption = new Option<string>("--custom-input", description: "Specify a custom file to use as problem input (relative to current working directory)");
var inputOption = new Option<string>("--input", description: "Select one of the registered inputs for the solution (see \"list inputs\" for details)");
var dayArgument = new Argument<string>("day", description: $"day number (Day## format) or '{AllDaysToken}'", getDefaultValue: () => AllDaysToken);
var partArgument = new Argument<string>("part", description: $"part number (Part# format) '{AllPartsToken}'", getDefaultValue: () => AllPartsToken);
var variantArgument = new Argument<string?>("variant", description: $"variant name or '{AllVariantsToken}'", getDefaultValue: () => null);
var listCmd = new Command("list", "List registered solutions and input files");
var listSolutionsCmd = new Command("solutions", "List known solutions");
var listInputsCmd = new Command("inputs", "List known input files");
var benchCmd = new Command("bench", "Benchmark a solution");
var warmupTimeOption = new Option<double>("--min-warmup-time", description: "Minimum time (in milliseconds) to run warmup rounds", getDefaultValue: () => 2000.0d);
var warmupRoundsOption = new Option<int>("--min-warmup-rounds", description: "Minimum number of warmup rounds", getDefaultValue: () => 10);
var sampleTimeOption = new Option<double>("--min-sample-time", description: "Minimum time (in milliseconds) to run sampling (benchmark) rounds", getDefaultValue: () => 10000.0d);
var sampleRoundsOption = new Option<int>("--min-sample-rounds", description: "Minimum number of sample (benchmark) rounds", getDefaultValue: () => 10);
var disableWarmupOption = new Option<bool>("--disable-warmup", description: "Disables the warmup rounds while benchmarking", getDefaultValue: () => false);

inputOption.AddAlias("-i");
runCmd.AddOption(inputOption);
customInputOption.AddAlias("-c");
runCmd.AddOption(customInputOption);
runCmd.AddArgument(dayArgument);
runCmd.AddArgument(partArgument);
runCmd.AddArgument(variantArgument);
runCmd.SetHandler(ExecuteRunCommand, inputOption, customInputOption, dayArgument, partArgument, variantArgument);
rootCmd.Add(runCmd);

listSolutionsCmd.AddAlias("solution");
listSolutionsCmd.AddArgument(dayArgument);
listSolutionsCmd.AddArgument(partArgument);
listSolutionsCmd.SetHandler(ExecuteListSolutionsCommand, dayArgument, partArgument);
listCmd.AddCommand(listSolutionsCmd);

listInputsCmd.AddAlias("input");
listInputsCmd.AddArgument(dayArgument);
listInputsCmd.AddArgument(partArgument);
listInputsCmd.AddArgument(variantArgument);
listInputsCmd.SetHandler(ExecuteListInputsCommand, dayArgument, partArgument, variantArgument);
listCmd.AddCommand(listInputsCmd);

rootCmd.Add(listCmd);

benchCmd.AddAlias("benchmark");
benchCmd.AddOption(inputOption);
benchCmd.AddOption(customInputOption);
benchCmd.AddOption(warmupTimeOption);
benchCmd.AddOption(warmupRoundsOption);
benchCmd.AddOption(sampleTimeOption);
benchCmd.AddOption(sampleRoundsOption);
benchCmd.AddOption(disableWarmupOption);
benchCmd.AddArgument(dayArgument);
benchCmd.AddArgument(partArgument);
benchCmd.AddArgument(variantArgument);
benchCmd.SetHandler(ExecuteBenchCommand);
rootCmd.Add(benchCmd);

// Parse command line
return await rootCmd.InvokeAsync(args);

// Adapter for list solutions command
async Task ExecuteListSolutionsCommand(string day, string part)
{
    var dayForRunner = ConvertIdentifier(day);
    var partForRunner = ConvertIdentifier(part);
    
    var runner = Setup();
    await runner.ExecuteListSolutionsCommand(dayForRunner, partForRunner);
}

// Adapter for list inputs command
async Task ExecuteListInputsCommand(string day, string part, string? variant)
{
    var dayForRunner = ConvertIdentifier(day);
    var partForRunner = ConvertIdentifier(part);
    var variantForRunner = ConvertIdentifier(variant ?? part);
    
    var runner = Setup();
    await runner.ExecuteListInputsCommand(dayForRunner, partForRunner, variantForRunner);
}

// Adapter for run command
async Task ExecuteRunCommand(string? input, string? inputFile, string day, string part, string? variant)
{
    var dayForRunner = ConvertIdentifier(day);
    var partForRunner = ConvertIdentifier(part);
    var variantForRunner = ConvertIdentifier(variant ?? part);

    var runner = Setup();
    await runner.ExecuteRunCommand(dayForRunner, partForRunner, variantForRunner, input, inputFile);
}

// Adapter for bench command.
// This has too many parameters to work in the normal form
async Task ExecuteBenchCommand(InvocationContext ctx)
{
    var day = ctx.ParseResult.GetValueForArgument(dayArgument);
    var part = ctx.ParseResult.GetValueForArgument(partArgument);
    var variant = ctx.ParseResult.GetValueForArgument(variantArgument);
    var input = ctx.ParseResult.GetValueForOption(inputOption);
    var inputFile = ctx.ParseResult.GetValueForOption(customInputOption);
    var disableWarmup = ctx.ParseResult.GetValueForOption(disableWarmupOption);
    var warmupTime = ctx.ParseResult.GetValueForOption(warmupTimeOption);
    var warmupRounds = ctx.ParseResult.GetValueForOption(warmupRoundsOption);
    var sampleTime = ctx.ParseResult.GetValueForOption(sampleTimeOption);
    var sampleRounds = ctx.ParseResult.GetValueForOption(sampleRoundsOption);
    
    var dayForRunner = ConvertIdentifier(day);
    var partForRunner = ConvertIdentifier(part);
    var variantForRunner = ConvertIdentifier(variant ?? part);
    
    var runner = Setup(builder => builder
        .ConfigureLogging(logging => logging
            .AddFilter("AdventOfCode", LogLevel.Warning)
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
    await runner.ExecuteBenchCommand(dayForRunner, partForRunner, variantForRunner, input, inputFile);
}

// Replace "all" with null
string? ConvertIdentifier(string id) => id.Equals(AllDaysToken, StringComparison.OrdinalIgnoreCase) ? null : id;

// Generic setup method
ISolutionRunner Setup(Action<IHostBuilder>? adapter = null)
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
        services.TryAddSingleton<IInputResolver, InputResolver>();
        services.TryAddSingleton<ISolutionRegistry, SolutionRegistry>();
        services.TryAddSingleton<ISolutionRunner, SolutionRunner>();
        services.TryAddSingleton<IBenchmarkRunner, BenchmarkRunner>();
    });

    // Put it all together
    var host = builder.Build();

    // Populate the SolutionRegistry
    var registry = host.Services.GetRequiredService<ISolutionRegistry>();
    registry.AddSolutions(typeof(ISolution).Assembly);
    
    // Return the SolutionRunner so that command handlers can call specific entry points
    return host.Services.GetRequiredService<ISolutionRunner>();
}