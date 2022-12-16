using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day15;

[Solution("Day15", "Part1")]
public class Day15Part1 : Day15
{
    // Big discovery - the answer is wrong if you forget to change this away from 10 :facepalm*
    private const int TargetRow = 2000000;
    
    private readonly ILogger<Day15Part1> _logger;
    public Day15Part1(ILogger<Day15Part1> logger)
        : base(long.MinValue, long.MaxValue)
    => _logger = logger;
    
    protected override void RunDay15(SensorGrid grid)
    {
        // Calculate how many positions cannot contain a beacon
        var beaconsOnRow = grid.Beacons.Count(b => b.Row == TargetRow);
        var rowLine = grid.GetPositionsThatCannotContainABeacon(TargetRow);
        var usedOnRow = rowLine.Area - beaconsOnRow;
        
        _logger.LogInformation("In row {TargetRow}, there are [{usedOnRow}] positions that cannot contain a beacon.", TargetRow, usedOnRow);
    }
}