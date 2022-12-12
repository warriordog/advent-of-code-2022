using AdventOfCode.Common;

namespace AdventOfCode.Day12;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
public abstract class Day12 : ISolution
{
    public void Run(string inputFile)
    {
        // Parse input into a hybrid grid.
        // Grid contains both the input heightmap and Dijkstra state.
        var grid = ParseInput(inputFile);

        // Run Dijkstra's algorithm to find paths
        RunDijkstra(grid);
        
        RunDay12(grid);
    }
    
    protected abstract void RunDay12(Grid grid);

    
    // We run Dijkstra in reverse, so that we can find the all the shortest paths from a -> ending point
    private static void RunDijkstra(Grid grid)
    {
        // Set distance for starting point.
        // This inverts the usual algorithm with "if v != source" in order to avoid the loop.
        grid.EndingNode.Distance = 0;
        
        // Set up queue
        var queue = new UniqueQueue<Point>();
        queue.Enqueue(grid.EndingPoint);

        // Run search
        while (queue.Count > 0)
        {
            var point = queue.Dequeue();
            
            RunDijkstraFor(grid, queue, point, Dir.Up);
            RunDijkstraFor(grid, queue, point, Dir.Right);
            RunDijkstraFor(grid, queue, point, Dir.Down);
            RunDijkstraFor(grid, queue, point, Dir.Left);
        }
    }
    
    private static void RunDijkstraFor(Grid grid, UniqueQueue<Point> queue, Point point, Dir dir)
    {
        // Check bounds
        if (dir == Dir.Up && point.Row < 1) return;
        if (dir == Dir.Down && point.Row >= grid.Height - 1) return;
        if (dir == Dir.Right && point.Col >= grid.Width - 1) return;
        if (dir == Dir.Left && point.Col < 1) return;
        
        // Check elevation
        var toPoint = point.GetNeighbor(dir);
        var to = grid[toPoint];
        var from = grid[point];
        if (from.Elevation - to.Elevation > 1) return; // Since we are working from the end, we can only go DOWN one step
            
        // Move is valid, compute the new distance
        var alt = from.Distance + 1; // Distance is always "one step".
        
        // Check if this is the new best route for neighbor
        if (alt < to.Distance)
        {
            to.Distance = alt;
            to.Previous = FlipDir(dir); // Setting this on the neighbor, so it needs to point backwards
            queue.Enqueue(toPoint);
        }
    }

    private static Dir FlipDir(Dir dir) => dir switch
    {
        Dir.Up => Dir.Down,
        Dir.Down => Dir.Up,
        Dir.Right => Dir.Left,
        Dir.Left => Dir.Right,
        _ => dir
    };

    // Abuse of out parameters, who cares
    private static Grid ParseInput(string inputFile)
    {
        var rows = new List<List<Node>>();
        Point? sp = null;
        Point? ep = null;

        var lines = inputFile
            .AsSpan()
            .TrimEnd()
            .SplitLines();

        // Read all input text
        foreach (var line in lines)
        {
            var rowArr = new List<Node>();
            for (var col = 0; col < line.Length; col++)
            {
                var chr = line[col];

                // Parse input value
                byte elevation;
                switch (chr)
                {
                    case 'S':
                        sp = new Point(rows.Count, col);
                        elevation = 0;
                        break;
                    case 'E':
                        ep = new Point(rows.Count, col);
                        elevation = 25;
                        break;
                    default:
                        elevation = (byte)(chr - 'a');
                        break;
                }
                
                // Create node
                rowArr.Add(new Node { Elevation = elevation });
            }
            rows.Add(rowArr);
        }

        // Validate findings
        if (sp == null)
            throw new ArgumentException("Input did not contain a starting point", nameof(inputFile));
        if (ep == null)
            throw new ArgumentException("Input did not contain an ending point", nameof(inputFile));
        
        // Construct grid
        return Grid.CreateFrom(rows, sp, ep);
    }

    protected static int CountDistanceToEnd(Grid grid, Point startingPoint)
    {
        var steps = 0;

        // Walk from the starting point to the ending point
        var current = startingPoint;
        while (current != grid.EndingPoint)
        {
            var currentNode = grid[current];
            
            // BMake sure that we have a path
            if (currentNode.Previous == Dir.None || currentNode.Distance == int.MaxValue)
                return int.MaxValue;

            // Move to previous node
            current = current.GetNeighbor(currentNode.Previous);
            steps++;
        }
        
        return steps;
    }
}

public class Grid
{
    public int Width { get; }
    public int Height { get; }
    
    public Point StartingPoint { get; }
    public Node StartingNode { get; }

    public Point EndingPoint { get; }
    public Node EndingNode { get; }
    
    private readonly Node[,] _data;

    public Grid(Node[,] data, int width, int height, Point startingPoint, Point endingPoint)
    {
        _data = data;
        Width = width;
        Height = height;
        StartingPoint = startingPoint;
        StartingNode = this[startingPoint];
        EndingPoint = endingPoint;
        EndingNode = this[endingPoint];
    }

    public Node this[Point p]
    {
        get => this[p.Row, p.Col];
        set => this[p.Row, p.Col] = value;
    }

    public Node this[int row, int col]
    {
        get => _data[row, col];
        set => _data[row, col] = value;
    }

    public static Grid CreateFrom(List<List<Node>> rows, Point startingPoint, Point endingPoint)
    {
        var height = rows.Count;
        var width = rows[0].Count;
        var data = new Node[height, width];
        for (var row = 0; row < height; row++)
        {
            var rowData = rows[row];
            for (var col = 0; col < width; col++)
            {
                data[row, col] = rowData[col];
            }
        }
        return new Grid(data, width, height, startingPoint, endingPoint);
    }
}

public class Node
{
    public byte Elevation { get; init; }
    public Dir Previous { get; set; } = Dir.None;
    public int Distance { get; set; } = int.MaxValue;
}

public record Point(int Row, int Col)
{
    public Point GetNeighbor(Dir direction) => direction switch
    {
        Dir.Up => this with { Row = Row - 1 },
        Dir.Down => this with { Row = Row + 1 },
        Dir.Right => this with { Col = Col + 1 },
        Dir.Left => this with { Col = Col - 1 },
        Dir.None => this,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), "Direction must be a valid value of Dir enum.")
    };

    public override string ToString() => $"({Row}, {Col})";
}

public enum Dir
{
    None,
    Up,
    Down,
    Right,
    Left
}