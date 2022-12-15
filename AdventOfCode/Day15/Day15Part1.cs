using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day15;

[Solution("Day15", "Part1")]
public class Day15Part1 : Day15
{
    private readonly ILogger<Day15Part1> _logger;
    public Day15Part1(ILogger<Day15Part1> logger) => _logger = logger;
    
    protected override void RunDay15(SensorGrid grid)
    {
        // Big discovery - the answer is wrong if you forget to change this away from 10 :facepalm*
        const int TargetRow = 2000000;

        // Calculate how many positions cannot contain a beacon
        var beaconsOnRow = grid.Sensors
            .Select(s => s.Beacon)
            .Distinct() // Only count each beacon once!!!!!!!!!!!!!!!!!
            .Count(b => b.Row == TargetRow);
        var rowLine = grid.Sensors.Aggregate(new CompoundHLine(), (line, s) => line + s.GetLineForRow(TargetRow));
        var usedOnRow = rowLine.Area - beaconsOnRow;
        
        _logger.LogInformation("In row {TargetRow}, there are {usedOnRow} positions that cannot contain a beacon.", TargetRow, usedOnRow);
    }
}