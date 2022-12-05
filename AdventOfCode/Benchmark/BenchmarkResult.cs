namespace AdventOfCode.Benchmark;

public class BenchmarkResult
{
    public bool WarmupWasRun { get; init; }
    public double TotalWarmupTimeMs { get; init; }
    public int TotalWarmupRounds { get; init; }
    
    public double TotalSampleTimeMs { get; init; }
    public int TotalSampleRounds { get; init; }
    
    public double AverageTimeMs { get; init; }
}