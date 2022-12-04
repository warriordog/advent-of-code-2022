using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Benchmark;

public class BenchmarkRunner
{
    private readonly Stopwatch _stopwatch;
    private readonly double _minWarmupTime;
    private readonly int _minWarmupRounds;
    private readonly double _minSampleTime;
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
        return new BenchmarkResult
        {
            TotalWarmupTimeMs = warmupMs,
            TotalWarmupRounds = warmupRounds,
            TotalSampleTimeMs = sampleMs,
            TotalSampleRounds = sampleRounds,
            AverageTimeMs = average
        };
    }
    
    private double RunRounds(ISolution solution, string input, int minRounds, double minTime, out int actualRounds)
    {
        var minTicks = MillisecondsToStopwatchTicks(minTime);
        
        var ticks = 0L;
        var rounds = 0;
        while (rounds < minRounds || ticks < minTicks)
        {
            _stopwatch.Restart();
            solution.Run(input);
            _stopwatch.Stop();

            ticks += _stopwatch.ElapsedTicks;
            rounds++;
        }
        
        actualRounds = rounds;
        return StopwatchTicksToMilliseconds(ticks);
    }

    // I hope these are right, I'm not great at math lol
    private static long MillisecondsToStopwatchTicks(double milliseconds) => (long)(Stopwatch.Frequency * (milliseconds / 1000.0d));
    private static double StopwatchTicksToMilliseconds(long ticks) => (ticks * 1000.0d) / Stopwatch.Frequency;
}

public class BenchmarkRunnerOptions
{
    public double MinWarmupTimeMs { get; set; } = 2000.0d;
    
    [Range(0, int.MaxValue)]
    public int MinWarmupRounds { get; set; } = 10;

    public double MinSampleTimeMs { get; set; } = 10000.0d;
    
    [Range(1, int.MaxValue)]
    public int MinSampleRounds { get; set; } = 10;
}