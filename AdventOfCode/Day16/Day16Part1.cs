using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day16;

[Solution("Day16", "Part1")]
public class Day16Part1 : Day16
{
    public Day16Part1(ILogger<Day16Part1> logger) : base(logger) {}
    
    protected override void RunDay16(Valve[] valves, Path bestPath)
    {
        var flow = bestPath.MinFlow; // MinFlow and MaxFlow should be the same now
        Logger.LogInformation("This approach will release [{flow}] pressure in 30 minutes.", flow);
    }
}