using System.CommandLine;
using AdventOfCode;
using AdventOfCode.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

const string AllDaysToken = "all";
const string AllPartsToken = "all";

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
    
    var runner = Setup(builder => builder
        .ConfigureServices(services => services
            .AddOptions<RunnerOptions>()
            .Configure(o => o.CustomInputFile = inputFile)
        ));
    await runner.ExecuteRunCommand(dayForRunner, partForRunner);
}

// Replace "all" with null
string? ConvertIdentifier(string id) => id.Equals(AllDaysToken, StringComparison.OrdinalIgnoreCase) ? null : id;

// Generic setup method
Runner Setup(Action<IHostBuilder>? adapter = null)
{
    var builder = Host.CreateDefaultBuilder();

    // Let caller have first dibs
    adapter?.Invoke(builder);
    
    // Populate defaults
    builder.ConfigureServices(services =>
    {
        services.TryAddTransient<Runner>();
        services.TryAddOptions<RunnerOptions>();
    });

    var host = builder.Build();
    return host.Services.GetRequiredService<Runner>();
}