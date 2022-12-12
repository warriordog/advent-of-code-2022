using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day12;

[Solution("Day12", "Part1")]
public class Day12Part1 : Day12
{
    private readonly ILogger<Day12Part1> _logger;

    public Day12Part1(ILogger<Day12Part1> logger) => _logger = logger;
    
    protected override void RunDay12(Grid grid)
    {
        var shortestPath = CountDistanceToEnd(grid, grid.StartingPoint);
        _logger.LogInformation("The shortest path requires [{num}] steps.", shortestPath);
    }
}