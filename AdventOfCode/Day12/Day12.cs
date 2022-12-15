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

        // Run breadth-first-search algorithm to find paths
        RunBFS(grid);
        
        RunDay12(grid);
    }
    
    protected abstract void RunDay12(Grid grid);

    
    // We run BFS in reverse, so that we can find the all the shortest paths from a -> ending point
    private static void RunBFS(Grid grid)
    {
        // Set up BFS
        var queue = new Queue<Point>();
        grid.EndingNode.Explored = true;
        queue.Enqueue(grid.EndingPoint);
        
        // Run BFS
        while (queue.Count > 0)
        {
            var point = queue.Dequeue();
            
            RunBFSFor(grid, queue, point, Direction.Up);
            RunBFSFor(grid, queue, point, Direction.Right);
            RunBFSFor(grid, queue, point, Direction.Down);
            RunBFSFor(grid, queue, point, Direction.Left);
        }
    }
    private static void RunBFSFor(Grid grid, Queue<Point> queue, Point point, Direction dir)
    {
        // Check bounds
        if (dir == Direction.Up && point.Row < 1) return;
        if (dir == Direction.Down && point.Row >= grid.Height - 1) return;
        if (dir == Direction.Right && point.Col >= grid.Width - 1) return;
        if (dir == Direction.Left && point.Col < 1) return;
        
        // Check if explored
        var toPoint = point.GetNeighbor(dir);
        var to = grid[toPoint];
        if (to.Explored) return;
        
        // Check elevation
        var from = grid[point];
        if (from.Elevation - to.Elevation > 1) return; // Since we are working from the end, we can only go DOWN one step

        // Explore the node
        to.Explored = true;
        to.Previous = FlipDir(dir); // Setting this on the neighbor, so it needs to point backwards
        queue.Enqueue(toPoint);
    }

    private static Direction FlipDir(Direction dir) => dir switch
    {
        Direction.Up => Direction.Down,
        Direction.Down => Direction.Up,
        Direction.Right => Direction.Left,
        Direction.Left => Direction.Right,
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
        return Grid.CreateFrom(rows, sp.Value, ep.Value);
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
            if (!currentNode.Explored || currentNode.Previous == Direction.None)
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
    public long Width { get; }
    public long Height { get; }
    
    public Point StartingPoint { get; }
    public Node StartingNode { get; }

    public Point EndingPoint { get; }
    public Node EndingNode { get; }
    
    private readonly Node[,] _data;

    public Grid(Node[,] data, long width, long height, Point startingPoint, Point endingPoint)
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

    public Node this[long row, long col]
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
    public Direction Previous { get; set; } = Direction.None;
    public bool Explored { get; set; } = false;
}