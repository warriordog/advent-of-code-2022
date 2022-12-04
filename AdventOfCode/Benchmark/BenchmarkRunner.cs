using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Benchmark;

public class BenchmarkRunner
{
    private readonly Stopwatch _stopwatch;
    private readonly long _minWarmupTime;
    private readonly int _minWarmupRounds;
    private readonly long _minSampleTime;
    private readonly int _minSampleRounds;

    public BenchmarkRunner(IOptions<BenchmarkRunnerOptions> options)
    {
        _stopwatch = Stopwatch.StartNew();
        _minWarmupTime = options.Value.MinWarmupTimeMs;
        _minWarmupRounds = options.Value.MinWarmupRounds;
        _minSampleTime = options.Value.MinSampleTimeMs;
        _minSampleRounds = options.Value.MinSampleRounds;
    }
    
    public BenchmarkResult ExecuteBenchmark(ISolution solution, string input)
    {
        // Warmup
        var warmupMs = RunRounds(solution, input, _minWarmupRounds, _minWarmupTime, out var warmupRounds);
        
        // Sample
        var sampleMs = RunRounds(solution, input, _minSampleRounds, _minSampleTime, out var sampleRounds);

        var average = sampleMs / sampleRounds;
        return new BenchmarkResult()
        {
            TotalWarmupTimeMs = warmupMs,
            TotalWarmupRounds = warmupRounds,
            TotalSampleTimeMs = sampleMs,
            TotalSampleRounds = sampleRounds,
            AverageTimeMs = average
        };
    }
    
    private double RunRounds(ISolution solution, string input, int minRounds, long minTime, out int actualRounds)
    {
        _stopwatch.Restart();

        var rounds = 0;
        while (rounds < minRounds || _stopwatch.ElapsedMilliseconds < minTime)
        {
            solution.Run(input);
            rounds++;
        }
        
        _stopwatch.Stop();
        actualRounds = rounds;
        return _stopwatch.Elapsed.TotalMilliseconds;
    }
}

public class BenchmarkRunnerOptions
{
    public long MinWarmupTimeMs { get; set; } = 100;
    
    [Range(0, int.MaxValue)]
    public int MinWarmupRounds { get; set; } = 100;

    [Range(1, long.MaxValue)]
    public long MinSampleTimeMs { get; set; } = 10000;
    
    [Range(1, int.MaxValue)]
    public int MinSampleRounds { get; set; } = 10;
}