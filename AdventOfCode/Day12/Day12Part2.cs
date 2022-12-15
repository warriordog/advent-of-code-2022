using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day12;

[Solution("Day12", "Part2")]
public class Day12Part2 : Day12
{

    private readonly ILogger<Day12Part2> _logger;

    public Day12Part2(ILogger<Day12Part2> logger) => _logger = logger;
    
    protected override void RunDay12(Grid grid)
    {
        // Find all potential starting points (elevation == 0 / 'a')
        var startingPoints = FindStartingPoints(grid);
        
        // Find the shortest path from any of the starting points
        var shortestPathLength = int.MaxValue;
        foreach (var startingPoint in startingPoints)
        {
            var distance = CountDistanceToEnd(grid, startingPoint);
            if (distance < shortestPathLength)
            {
                shortestPathLength = distance;
            }
        }
        
        _logger.LogInformation("The shortest path requires [{num}] steps.", shortestPathLength);
    }
    
    private static IEnumerable<Point> FindStartingPoints(Grid grid)
    {
        for (var row = 0; row < grid.Height; row++)
        {
            for (var col = 0; col < grid.Width; col++)
            {
                if (grid[row, col].Elevation == 0)
                {
                    yield return new Point(row, col);
                }
            }
        }
    }
}