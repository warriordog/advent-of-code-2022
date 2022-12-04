namespace AdventOfCode.Benchmark;

public interface IBenchmarkTimer
{
    /// <summary>
    /// Total time between the most recent call to Start() and the most recent call to Stop().
    /// Should be zero if the timer has not run.
    /// </summary>
    public double ElapsedTime { get; }
    
    public void Start();
    public void Stop();
}