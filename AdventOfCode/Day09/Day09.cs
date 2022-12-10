using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day09;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
[InputFile("challenge_1m.txt", InputFileType.Challenge)]
[InputFile("challenge_10m.txt", InputFileType.Challenge)]
public abstract class Day09 : ISolution
{
    private readonly ILogger _logger;

    private readonly int _numKnots;
    
    protected Day09(ILogger logger, int numKnots)
    {
        if (numKnots < 1) throw new ArgumentOutOfRangeException(nameof(numKnots), "There must be at least two knots");

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
        
        // Track number of times that the tail has been in a particular position
        var tailPositions = new HashSet<Point>()
        {
            // Make sure we count the starting point
            new(0, 0)
        };

        // Process each motion
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
                tailPositions.Add(tail.GetPoint());
            }
        }
        
        _logger.LogInformation("The tail visited [{unique}] unique locations.", tailPositions.Count);
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
    }
}

public record Knot(int Row, int Col, byte Index)
{
    public int Row { get; set; } = Row;
    public int Col { get; set; } = Col;

    public Point GetPoint() => new(Row, Col);
    
    public override string ToString() => $"{Index}@[{Row},{Col}]";
}

// ReSharper disable twice NotAccessedPositionalProperty.Global
public readonly record struct Point(int Row, int Col);