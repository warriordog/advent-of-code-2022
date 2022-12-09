using System.Text;
using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day09;

public abstract class Day09 : ISolution
{
    private readonly bool _debug;
    private readonly ILogger _logger;

    private readonly int _numKnots;
    
    protected Day09(bool debug, ILogger logger, int numKnots)
    {
        if (numKnots < 1) throw new ArgumentOutOfRangeException(nameof(numKnots), "There must be at least two knots");
        
        _debug = debug;
        _logger = logger;
        _numKnots = numKnots;
    }

    public void Run(string inputFile)
    {
        var inputLines = inputFile
            .AsSpan()
            .TrimEnd()
            .SplitLines();

        // Create all the knots
        var knots = new Knot[_numKnots];
        for (var i = 0; i < _numKnots; i++)
        {
            knots[i] = new Knot(0, 0, (byte)i);
        }
        var head = knots[0];
        var tail = knots[_numKnots - 1];
        
        // Track number of times that tail has been in a particular position
        var tailPositions = new Plane<int>
        {
            // Make sure we count the starting point
            [0, 0] = 1
        };

        // Process each motion
        var lineNum = 0;
        foreach (var line in inputLines)
        {
            // Parse line
            var dir = line[0];
            var distance = int.Parse(line[2..]);
            
            // Process each step
            for (var step = 0; step < distance; step++)
            {
                // Move head
                MoveHead(dir, head);

                // Move the rest of the knots
                var previous = head;
                for (var k = 1; k < knots.Length; k++)
                {
                    var knot = knots[k];
                
                    // Move it towards the previous knot
                    MoveKnot(knot,previous);

                    previous = knot;
                }
                
                // Track new tail position
                tailPositions[tail.Row, tail.Col]++;

                PrintStep(lineNum, step, head, tail, knots, tailPositions);
            }

            lineNum++;
        }

        // Count up visited positions
        var totalVisited = 0;
        var uniqueVisited = 0;
        for (var y = tailPositions.YMin; y <= tailPositions.YMax; y++)
        {
            var xAxis = tailPositions[y];
            if (xAxis == null) continue;
            
            for (var x = xAxis.Min; x <= xAxis.Max; x++)
            {
                totalVisited += xAxis[x];
                
                if (xAxis[x] > 0)
                {
                    uniqueVisited++;
                }
            }
        }
        
        _logger.LogInformation("The tail visited [{unique}] unique locations a total of {total} times.", uniqueVisited, totalVisited);
    }
    
    private void PrintStep(int motion, int step, Knot head, Knot tail, Knot[] knots, Plane<int> tailPositions)
    {
        if (!_debug) return;

        var yMax = Math.Max(tailPositions.YMax, head.Row);
        var yMin = Math.Min(tailPositions.YMax, head.Row);
        var xMax = Math.Max(tailPositions.XMax, head.Col);
        var xMin = Math.Min(tailPositions.XMin, head.Col);
        
        var sb = new StringBuilder();
        for (var y = yMax; y >= yMin; y--)
        {
            for (var x = xMin; x <= xMax; x++)
            {
                // Head
                if (y == head.Row && x == head.Col)
                {
                    sb.Append('H');
                    continue;
                }
                
                // Tail
                if (y == tail.Row && x == tail.Col)
                {
                    sb.Append('T');
                    continue;
                }
                
                // Intermediate
                var foundIntermediate = false;
                for (var i = 1; i < knots.Length - 1; i++)
                {
                    var knot = knots[i];
                    if (y == knot.Row && x == knot.Col)
                    {
                        sb.Append(knot.Index);
                        foundIntermediate = true;
                        break;
                    }
                }
                if (foundIntermediate)
                {
                    // Please vote for this issue: https://github.com/dotnet/csharplang/discussions/6634
                    break;
                }
                
                
                // Visited
                if (tailPositions[y, x] > 0)
                {
                    sb.Append('#');
                    continue;
                }
                
                // Starting point
                if (x == 0 && y == 0)
                {
                    sb.Append('s');
                    continue;
                }
                
                // Unvisited (fallback)
                sb.Append('.');
            }

            sb.Append('\n');
        }

        _logger.LogTrace("Motion {motion} step {step} resulted in a grid from [{yMax},{xMin}] to [{yMin},{xMax}]:\n{grid}", motion, step, yMax, xMin, yMin, xMax, sb);
    }

    private static void MoveHead(char dir, Knot head)
    {
        if (dir == 'U') head.Row++;
        if (dir == 'D') head.Row--;
        if (dir == 'R') head.Col++;
        if (dir == 'L') head.Col--;
    }

    private void MoveKnot(Knot knot, Knot previous)
    {
        // Check the distance.
        // If we're close enough then don't move at all.
        if (Math.Abs(previous.Row - knot.Row) < 2 && Math.Abs(previous.Col - knot.Col) < 2)
            return;

        if (previous.Row > knot.Row) knot.Row++;
        if (previous.Row < knot.Row) knot.Row--;

        if (previous.Col > knot.Col) knot.Col++;
        if (previous.Col < knot.Col) knot.Col--;

        if (_debug)
        {
            _logger.LogTrace("Moving knot {knot} to keep up with previous knot {previous}", knot, previous);
        }
    }
}

public record Knot(int Row, int Col, byte Index)
{
    public int Row { get; set; } = Row;
    public int Col { get; set; } = Col;

    public override string ToString() => $"{Index}@[{Row},{Col}]";
}