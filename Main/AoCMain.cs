using AdventOfCode;
using Main.Benchmark;
using Main.Services;
using Main.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using static Microsoft.Extensions.Hosting.Host;

namespace Main;

public sealed class AoCMain : IAsyncDisposable
{
    public IHost Host { get; }
    public ISolutionRunner Runner { get; }
    
    private AoCMain(IHost host)
    {
        Host = host;
        Runner = host.Services.GetRequiredService<ISolutionRunner>();
    }

    public async ValueTask DisposeAsync()
    {
        await Host.StopAsync();
        await Host.AsAsyncDisposable().DisposeAsync();
    }


    public static async Task<AoCMain> Start(bool verbose = false, Action<IHostBuilder>? adapter = null, CancellationToken? ct = null)
    {
        // Static import to avoid clash with Host property above
        var builder = CreateDefaultBuilder();

        // Enable console support
        builder.UseConsoleLifetime(console => 
            console.SuppressStatusMessages = true
        );
    
        // Set config first so that adapter can override
        builder.ConfigureLogging(logging => logging
            .ClearProviders()
            .AddConsoleFormatter<SolutionConsoleFormatter, ConsoleFormatterOptions>()
            .AddConsole(console =>
            {
                console.FormatterName = nameof(SolutionConsoleFormatter);
                console.MaxQueueLength = 1; // This is CRITICAL to prevent lost logs!
            })
            .SetMinimumLevel(verbose ? LogLevel.Debug : LogLevel.Information)
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

        // Create the host
        var host = builder.Build();
    
        // Flush IO on exit
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopped.Register(() =>
        {
            // Flush these bad boys to prevent truncated output
            Console.Out.Flush();
            Console.Error.Flush();
        });

        // Populate the SolutionRegistry
        var registry = host.Services.GetRequiredService<ISolutionRegistry>();
        registry.AddSolutions(typeof(ISolution).Assembly);
    
        // Start up host
        if (ct != null)
            await host.StartAsync(ct.Value);
        else
            await host.StartAsync();

        return new AoCMain(host);
    }
}