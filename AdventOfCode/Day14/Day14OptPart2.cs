using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day14;

[Solution("Day14", "Part2", "Optimized")]
public class Day14OptPart2 : Day14Opt
{

    public Day14OptPart2(ILogger<Day14OptPart2> logger) : base(logger, true) {}
    
    protected override int RunSimulation(OptimizedScanData scanData)
    {
        var sandCount = 0;

        // Queue to hold pending positions to scan from
        var sandQueue = new Queue<Point>();
        sandQueue.Enqueue(new Point(0, 500));
        
        // Process until there are none left
        while (sandQueue.TryDequeue(out var point))
        {
            // Skip if this point is already processed or invalid
            if (scanData[point] != Matter.Air)
                continue;
            
            // Add everything from here down
            var next = point;
            while (scanData[next] == Matter.Air)
            {
                // Queue down-left and down-right
                sandQueue.Enqueue(next.MoveBy(1, -1));
                sandQueue.Enqueue(next.MoveBy(1, 1));
                
                // Add the next grain of sand
                scanData[next] = Matter.Sand;
                next = next.MoveBy(1, 0);
                sandCount++;
            }
        }

        return sandCount;
    }
}