using System.Text;
using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day14;

public abstract class Day14Opt : Day14<OptimizedScanData>
{
    private readonly bool _hasFloor;
    protected Day14Opt(ILogger logger, bool hasFloor) : base(logger) => _hasFloor = hasFloor;

    // TODO every part of this can be optimized with a custom loop, but rn this is a very small part of total runtime
    protected override OptimizedScanData ParseInput(SpanLineSplitter input)
    {
        // Read all the lines
        var points = new List<Point>();
        foreach (var line in input)
        {
            // Get all major points in the line
            var linePoints = line
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
            
            // Expand to all sub-points
            var previous = default(Point);
            var isFirst = true;
            foreach (var point in linePoints)
            {
                // Special case - don't skip the first point
                if (!isFirst)
                {
                    var midPoints = GetPath(previous, point);
                    points.AddRange(midPoints);
                }
                
                isFirst = false;
                previous = point;
            }
        }
        
        // Compute bounds and create ScanData
        var rowMax = points.Max(p => p.Row);
        if (_hasFloor)
            rowMax += 2;
        var colMin = 500 - rowMax;
        var colMax = 500 + rowMax;

        // Create ScanData and populate with all points
        var scanData = new OptimizedScanData(rowMax, colMin, colMax);
        foreach (var point in points)
        {
            scanData[point] = Matter.Rock;
        }
        
        // Populate the floor
        if (_hasFloor)
        {
            for (var col = colMin; col <= colMax; col++)
            {
                scanData[rowMax, col] = Matter.Rock;
            }
        }

        return scanData;
    }

    private static IEnumerable<Point> GetPath(Point from, Point to)
    {
        if (from.Row != to.Row)
        {
            if (from.Col != to.Col)
            {
                throw new ArgumentException("Row and column cannot both be different - only horizontal moves are supported");
            }

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

public class OptimizedScanData
{
    private readonly Matter[,] _matter;
    private readonly long _rowMax;
    private readonly long _colMin;
    private readonly long _colMax;
    
    public OptimizedScanData(long rowMax, long colMin, long colMax)
    {
        _colMin = colMin;
        _colMax = colMax;
        _rowMax = rowMax;
        _matter = new Matter[rowMax + 1, (colMax - colMin) + 1];
    }

    public Matter this[Point point]
    {
        get => this[point.Row, point.Col];
        set => this[point.Row, point.Col] = value;
    }
    
    public Matter this[long row, long col]
    {
        get
        {
            // If out of bounds, then return Void
            if (!IsInBounds(row, col))
            {
                return Matter.Void;
            }
            
            // Normalize column position
            var colIndex = col - _colMin;
            return _matter[row, colIndex];
        }
        set
        {
            if (!IsInBounds(row, col))
            {
                throw new ArgumentException($"Point ({row},{col}) is not in range ({0},{_colMin}) - ({_rowMax},{_colMax})");
            }

            // Normalize column position
            var colIndex = col - _colMin;
            _matter[row, colIndex] = value;
        }
    }

    private bool IsInBounds(long row, long col) =>
        row >= 0 &&
        row <= _rowMax &&
        col >= _colMin &&
        col <= _colMax;

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var row = 0; row <= _rowMax; row++)
        {
            if (row > 0)
            {
                sb.Append('\n');
            }
            
            for (var col = _colMin; col <= _colMax; col++)
            {
                sb.Append(this[row, col].GetDisplayChar());
            }
        }
        return sb.ToString();
    }
}