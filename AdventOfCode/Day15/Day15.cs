using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day15;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
public abstract class Day15 : ISolution
{
    private static readonly Regex ParseRegex = new(@"Sensor at x=([-\d]+), y=([-\d]+): closest beacon is at x=([-\d]+), y=([-\d]+)", RegexOptions.Compiled);
    
    public void Run(string inputFile)
    {
        // Not bothering with Spans today...
        var sensors = ParseRegex
            .Matches(inputFile)
            .Select(m =>
            {
                var sensorX = long.Parse(m.Groups[1].Value);
                var sensorY = long.Parse(m.Groups[2].Value);
                var beaconX = long.Parse(m.Groups[3].Value);
                var beaconY = long.Parse(m.Groups[4].Value);
                return new Sensor(
                    // (Row,Col) == (Y,X) !!!!
                    new Point(sensorY, sensorX),
                    new Point(beaconY, beaconX)
                );
            })
            .ToArray();

        // Extract metadata and construct grid
        var sensorGrid = new SensorGrid(sensors);
        
        RunDay15(sensorGrid);
    }
    
    protected abstract void RunDay15(SensorGrid grid);
}

public class SensorGrid
{
    public Sensor[] Sensors { get; }
    
    public Point[] Beacons { get; }
    
    public SensorGrid(Sensor[] sensors)
    {
        Sensors = sensors;
        Beacons = sensors
            .Select(s => s.Beacon)
            .Distinct() // Only count each beacon once!!!!!!!!!!!!!!!!!
            .ToArray();
    }

    public CompoundHLine GetPositionsThatCannotContainABeacon(long row) => Sensors
        .Aggregate(new CompoundHLine(), (line, s) =>
            line + s.GetLineForRow(row));
}

public class Sensor
{
    public Point Position { get; }
    public Point Beacon { get; }
    public long Distance { get; }
    
    public Sensor(Point position, Point beacon)
    {
        Position = position;
        Beacon = beacon;
        Distance = Math.Abs(position.Row - beacon.Row) + Math.Abs(position.Col - beacon.Col);
    }

    public HLine GetLineForRow(long row)
    {
        // Compute the distance between target row and this sensor's row
        var distToRow = Math.Abs(Position.Row - row);
        
        // The below algorithm will turn negative unless we cap it here
        if (distToRow > Distance)
        {
            // This is a zero-length line
            return new HLine(Position.Col, Position.Col);
        }
        
        // Compute used area for target
        var distInvert = Distance - distToRow; // Result should decrease as distToRow approaches Distance
        var start = Position.Col - distInvert;
        var end = Position.Col + distInvert;
        return new HLine(start, end);
    }
}

public class CompoundHLine
{
    public IReadOnlyList<HLine> Segments => _segments;
    private List<HLine> _segments = new();

    public long Area => _segments.Aggregate(0L, (sum, seg) => sum + seg.Area);

    public void AddLine(HLine line)
    {
        // If new line is empty, then ignore
        if (line.Distance < 1)
        {
            return;
        }
        
        // If the compound is empty, then just add and return
        if (!_segments.Any())
        {
            _segments.Add(line);
            return;
        }
        
        // If new line is fully inside an existing line, then skip it entirely.
        if (_segments.Any(seg => seg.Start <= line.Start && seg.End >= line.End))
        {
            return;
        }

        // Otherwise we ned to merge in.
        var newSegments = new List<HLine>();
        foreach (var segment in _segments)
        {
            // If out of range, then copy as-is
            if (segment.Start > line.End || segment.End < line.Start)
            {
                newSegments.Add(segment);
                continue;
            }

            // If fully encapsulated by new line, then exclude it
            if (segment.Start >= line.Start && segment.End <= line.End)
            {
                continue;
            }
            
            // If partially overlapping at the start
            if (segment.End >= line.Start && segment.End <= line.End)
            {
                // Extend line backwards to include this segment
                line = line with { Start = segment.Start };
                continue;
            }
            
            // If partially overlapping at the end
            if (segment.Start >= line.Start && segment.Start <= line.End)
            {
                // Extend line forwards to include this segment
                line = line with { End = segment.End };
                continue;
            }

            throw new ApplicationException("Algorithm failure - line slipped through the cracks and was not matched");
        }
        
        // Insert new line
        var insertIdx = newSegments.FindIndex(seg => seg.Start > line.End);
        if (insertIdx > -1)
            newSegments.Insert(insertIdx, line);
        else
            newSegments.Add(line);
        
        // Update segments
        _segments = newSegments;
    }

    public CompoundHLine Clamp(long min, long max)
    {
        int removeIdx;
        
        // Remove any past the max
        while ((removeIdx = _segments.FindLastIndex(s => s.Start >= max)) > -1)
        {
            _segments.RemoveAt(removeIdx);
        }
        
        // Remove any before the min
        while ((removeIdx = _segments.FindIndex(s => s.End <= min)) > -1)
        {
            _segments.RemoveAt(removeIdx);
        }

        if (_segments.Count > 0)
        {
            // Extend the new end to the max
            _segments[^1] = _segments[^1] with { End = max };

            // Extend the new start to the min
            _segments[0] = _segments[0] with { Start = min };
        }

        // Return itself for chaining
        return this;
    }

    public static CompoundHLine operator +(CompoundHLine compound, HLine line)
    {
        compound.AddLine(line);
        return compound;
    }
}

public readonly record struct HLine(long Start, long End)
{
    // Manhattan distance
    public long Distance => Math.Abs(End - Start);
    public long Area => Distance + 1;
}