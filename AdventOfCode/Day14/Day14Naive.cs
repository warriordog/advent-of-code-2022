using System.Text;
using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day14;

public abstract class Day14Naive : Day14<NaiveScanData>
{
    private readonly bool _includeFloor;
    protected Day14Naive(ILogger logger, bool includeFloor) : base(logger) => _includeFloor = includeFloor;

    protected override int RunSimulation(NaiveScanData scanData)
    {
        var sandCount = 0;
        
        while (DropSandFrom(scanData, new Point(0, 500)))
        {
            sandCount++;
        }

        return sandCount;
    }

    private static bool DropSandFrom(NaiveScanData data, Point point)
    {
        // Bail out if the starting point is already full
        if (data[point] != Matter.Air)
            return false;

        // Move the point until we find the landing place.
        while (true)
        {
            // First try to fall straight down
            point = point.MoveBy(1, 0);
            if (data[point] == Matter.Void) return false;
            if (data[point] == Matter.Air) continue;

            // Next try diagonal left
            point = point.MoveBy(0, -1); // Move by (0,-1) bc its relative from (1,0)
            if (data[point] == Matter.Void) return false;
            if (data[point] == Matter.Air) continue;

            // Next try diagonal right
            point = point.MoveBy(0, 2); // Move by (0,2) bc its relative from (1,-1)
            if (data[point] == Matter.Void) return false;
            if (data[point] == Matter.Air) continue;
            
            // Success - we found where to place the sand
            var sandPoint = point.MoveBy(-1, -1); // Move by (-1,-1) to return to the starting point. Checks have moved it by (1,1) at this point.
            data[sandPoint] = Matter.Sand;
            return true;
        }
    }
    
    protected override NaiveScanData ParseInput(SpanLineSplitter input)
    {
        var scanData = new NaiveScanData(_includeFloor);
        
        // Populate the dictionary
        foreach (var line in input)
        {
            // Not efficient, but can be optimized once we have SpanExtensions.Split()
            var points = line
                .ToString()
                .Split(" -> ")
                .Select(point =>
                {
                    var coords = point
                        .Split(',')
                        .Select(int.Parse)
                        .ToArray();
                    return new Point(coords[1], coords[0]);
                });

            // Draw the line
            var previous = default(Point);
            var isFirst = true;
            foreach (var point in points)
            {
                // Special case - don't draw to the first point
                if (!isFirst)
                {
                    // Draw from previous to current
                    foreach (var midPoint in GetPath(previous, point))
                    {
                        scanData[midPoint] = Matter.Rock;
                    }
                }
                
                isFirst = false;
                previous = point;
            }
        }

        return scanData;
    }
    
    private static IEnumerable<Point> GetPath(Point from, Point to)
    {
        if (from.Row != to.Row)
        {
            if (from.Col != to.Col)
                throw new ArgumentException("Row and column cannot both be different - only horizontal moves are supported");
            
            // Vertical line
            var rowMin = Math.Min(from.Row, to.Row);
            var rowMax = Math.Max(from.Row, to.Row);
            for (var row = rowMin; row <= rowMax; row++)
            {
                yield return new Point(row, from.Col);
            }
        }
        else
        {
            // Horizontal line
            var colMin = Math.Min(from.Col, to.Col);
            var colMax = Math.Max(from.Col, to.Col);
            for (var col = colMin; col <= colMax; col++)
            {
                yield return new Point(from.Row, col);
            }
        }
    }
}


public class NaiveScanData
{
    private readonly Dictionary<Point, Matter> _pointContents = new();
    private int _rowMin; // This one starts at zero
    private int _rowMax = int.MinValue;
    private int _colMin = int.MaxValue;
    private int _colMax = int.MinValue;
    private int EffectiveRowMax => _hasFloor ? _rowMax + 2 : _rowMax;
    
    private readonly bool _hasFloor;
    public NaiveScanData(bool hasFloor) => _hasFloor = hasFloor;

    public Matter this[Point point]
    {
        get
        {
            // If this is in the floor, then return rock
            if (_hasFloor && point.Row >= EffectiveRowMax)
            {
                return Matter.Rock;
            }
            
            // If out of bounds, then return Void
            if (!IsInBounds(point))
            {
                return Matter.Void;
            }

            // Default to Air
            if (!_pointContents.TryGetValue(point, out var matter))
            {
                matter = Matter.Air;
            }

            return matter;
        }
        set
        {
            // Record the matter
            _pointContents[point] = value;
            
            // Update bounds
            if (point.Col < _colMin)
                _colMin = point.Col;
            if (point.Col > _colMax)
                _colMax = point.Col;
            
            // Vertical bounds can only be changed by rocks.
            // Otherwise, falling sand can "push" the floor infinitely downward.
            if (value == Matter.Rock)
            {
                if (point.Row < _rowMin)
                    _rowMin = point.Row;
                if (point.Row > _rowMax)
                    _rowMax = point.Row;
            }
        }
    }

    private bool IsInBounds(Point point)
    {
        // These checks always apply
        if (point.Row < _rowMin || point.Row > EffectiveRowMax)
            return false;
        
        // These checks only apply when we have no floor
        if (!_hasFloor && (point.Col < _colMin || point.Col > _colMax))
            return false;

        return true;
    }

    public override string ToString()
    {
        // Failsafe - if nothing has been added, then this will try to load 2^32 * 2^16 * 2 bytes into memory!
        if (_pointContents.Count == 0)
        {
            return "";
        }
        
        var sb = new StringBuilder();
        for (var row = _rowMin; row <= EffectiveRowMax; row++)
        {
            if (row > _rowMin)
            {
                sb.Append('\n');
            }
            
            for (var col = _colMin; col <= _colMax; col++)
            {
                sb.Append(this[new Point(row, col)].GetDisplayChar());
            }
        }
        return sb.ToString();
    }
}