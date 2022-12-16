using System.Numerics;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day15;

[Solution("Day15", "Part2")]
public class Day15Part2 : Day15
{
    private const long MaxCoordinate = 4000000L;
    private const long TuningMultiplier = 4000000L;
    
    private readonly ILogger<Day15Part2> _logger;
    public Day15Part2(ILogger<Day15Part2> logger)
        : base(0L, MaxCoordinate)
    => _logger = logger;
    
    protected override void RunDay15(SensorGrid grid)
    {
        var (row, col) = FindBeaconFrequency(grid);
        var frequency = (col * TuningMultiplier) + row;
        _logger.LogInformation("The distress beacon is at ({row}, {col}) with tuning frequency of [{frequency}]", row, col, frequency);
    }
    
    private static (BigInteger Row, BigInteger Col) FindBeaconFrequency(SensorGrid grid)
    {
        // Brute force each Y coordinate
        for (var row = 0; row <= MaxCoordinate; row++)
        {
            var rowLine = grid.GetPositionsThatCannotContainABeacon(row);

            // There's only one possible space, so if we find any gap then that's the correct spot
            if (rowLine.Area == MaxCoordinate) // amount will be EQUAL because start/end are inclusive!
            {
                var col = FindGap(rowLine);

                var bigRow = new BigInteger(row);
                var bigCol = new BigInteger(col);
                return (bigRow, bigCol);
            }
        }

        throw new ApplicationException("Algorithm failure - reached the end of the search area without finding the beacon");
    }
    private static long FindGap(CompoundHLine line)
    {
        if (line.Area != MaxCoordinate)
        {
            throw new ApplicationException("Algorithm failure - line does not have exactly one gap");
        }
        
        // With only one possible gap, there are only three possible configurations:
        // 0.......E
        // [      ]. // 1 segment, gap at the end
        // .[      ] // 1 segment, gap at the start
        // [  ].[  ] // 2 segments, gap in the middle
        
        // Configuration 3
        if (line.Segments.Count == 2)
        {
            var first = line.Segments[0];
            return first.End + 1;
        }
        
        // Configurations 1/2
        if (line.Segments.Count == 1)
        {
            var seg = line.Segments[0];
            return seg.Start > 0 ? 0 : 4000000;
        }
        
        // Impossible configuration
        throw new ApplicationException("Algorithm failure: line has invalid number of segments");
    }
}