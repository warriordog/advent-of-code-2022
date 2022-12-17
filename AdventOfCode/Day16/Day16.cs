using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day16;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
public abstract class Day16 : ISolution
{
    private static readonly Regex ParseRegex = new(@"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]+)", RegexOptions.Compiled);

    protected readonly ILogger Logger;
    protected Day16(ILogger logger) => Logger = logger;
    
    public void Run(string inputFile)
    {
        var valves = ParseInput(inputFile);

        var bestPath = FindBestPath(valves);
        Logger.LogDebug("The best path is:\n{path}", bestPath.TracePath(valves));

        RunDay16(valves, bestPath);
    }
    
    private static Path FindBestPath(Valve[] valves)
    {
        // Queue of in-progress paths.
        // This is loosely prioritized based on MinFlow.
        // Paths with a higher MinFlow are more likely to be popped.
        var queue = new PathQueue();
        
        // Create a starting path and run DFS to seed the queue 
        var startingPath = CreateStartingPath(valves);
        RunToEnd(queue, valves, startingPath);
        
        // This is the best path found so far.
        // Will be updated as the algorithm continues.
        // If a path's MaxFlow drops below this path's MinFlow, then it will be discard.
        // If a path's MinFlow exceeds this path's MinFlow, then it will be replaced.
        var bestPath = startingPath;
        
        // Run hybrid BFS/DFS over the queue until we find the best path
        while (queue.TryPop(out var path))
        {
            // Skip (discard) if path is below threshold
            if (path.MaxFlow <= bestPath.MinFlow)
                continue;
            
            // Progress the next path.
            // RunStep is responsible for re-queuing (if needed).
             RunStep(queue, valves, path);

            // Swap bestPath if this is better.
            if (IsBetterThan(path, bestPath))
                bestPath = path;
        }

        return bestPath;
    }
    private static bool IsBetterThan(Path newPath, Path bestPath)
    {
        // Better MinFlow == better
        if (newPath.MinFlow > bestPath.MinFlow)
            return true;
        
        // Same MinFlow but higher MaxFlow == better
        if (newPath.MinFlow == bestPath.MinFlow && newPath.MaxFlow > bestPath.MaxFlow)
            return true;

        return false;
    }

    private static Path CreateStartingPath(Valve[] valves)
    {
        var startingValve = Array.Find(valves, v => v.ID == "AA");
        if (startingValve == null)
            throw new ArgumentException("Valves array is missing the starting valve", nameof(valves));

        return new Path(startingValve, valves.Length);
    }

    private static void RunToEnd(PathQueue queue, Valve[] valves, Path path, Path? bestPath = null)
    {
        // This will greedy-match all the way to the end.
        // The resulting path will have reasonable baseline stats to guide the real search.
        while (path.CanContinue)
        {
            // Stop if we become worse than the best path
            if (bestPath != null && path.MaxFlow <= bestPath.MinFlow)
                break;

            // This will queue up a bunch of duplicates, but they will already be terminated so the main loop will skip over them.
            RunStep(queue, valves, path);
        }
    }

    private static void RunStep(PathQueue queue, Valve[] valves, Path path)
    {
        // Optimization - there are no possible moves if the path is out of time.
        if (!path.CanContinue)
            return;
        
        // Optimization - there are no useful moves if all useful valves are open
        if (AreAllUsefulValvesOpen(valves, path))
        {
            path.Terminate();
            return;
        }
        
        // This will be a list of all possible moves.
        // The most desirable moves are first.
        var moves = GetPossibleMoves(valves, path);

        // Stop if there are no possible moves.
        if (moves.Count < 1)
        {
            path.Terminate();
            return;
        }
        
        // Otherwise, queue up the path because it will definitely move.
        // However, actually making the move needs to wait until the path is cloned for all the branches.
        // We just put this first to ensure that the greedy path runs next.
        queue.Push(path);
        
        // If we have multiple moves, then queue branches
        for (var i = 1; i < moves.Count; i++)
        {
            var move = moves[i];
            var clone = path.Clone();
            RunMoveSet(valves, clone, move);
            queue.Push(clone);
        }
        
        // Now we can finally run the greedy path.
        // Remember, it has already been queued.
        var greedyMove = moves[0];
        RunMoveSet(valves, path, greedyMove);
    }
    
    private static bool AreAllUsefulValvesOpen(Valve[] valves, Path path)
    {
        for (var index = 0; index < valves.Length; index++)
        {
            // If the valve has flow and is closed, then there is at least one useful valve.
            if (valves[index].FlowRate > 0 && !path.ValveStatuses[index])
            {
                return false;
            }
        }

        return true;
    }

    private static void RunMoveSet(Valve[] valves, Path path, MoveSet moveSet)
    {
        foreach (var move in moveSet)
        {
            path.MakeMove(valves, move);
        }
    }
    
    private static List<MoveSet> GetPossibleMoves(Valve[] valves, Path path)
    {
        var moves = new List<MoveSet>();
        
        // Run in valve order to ensure that the greedy path is checked first.
        // We want to quickly scale up MinFlow in order to reduce the search space
        for (var targetIndex = 0; targetIndex < valves.Length; targetIndex++)
        {
            // Don't move to itself
            if (targetIndex == path.Position.ValveIndex)
                continue;

            // Skip if valve has already been opened.
            if (path.ValveStatuses[targetIndex])
                continue;
            
            // Don't move to a valve with no flow
            if (valves[targetIndex].FlowRate < 1)
                continue;

            // Get the full path to the target valve
            var pathToValve = path.Position.PathTo[targetIndex];
            var distance = pathToValve.Length;

            // Skip if this valve is too far away
            if (path.TimeUsed + distance > 30)
                continue;

            // Queue up the move
            var move = CreateMoveSet(path, pathToValve, targetIndex);
            moves.Add(move);
        }

        return moves;
    }

    private static MoveSet CreateMoveSet(Path path, int[] pathToTarget, int target)
    {
        var set = new MoveSet();

        foreach (var valve in pathToTarget)
        {
            // Don't open the valve if its already been opened!
            var open = valve == target && !path.ValveStatuses[valve];
            
            var move = new Move(valve, open);
            set.Add(move);
        }
        
        return set;
    }

    protected abstract void RunDay16(Valve[] valves, Path bestPath);

    private static Valve[] ParseInput(string inputFile)
    {

        
        // Generate an ordered list of Valves in descending order by flow rate
        var valveData = ParseRegex
            .Matches(inputFile)
            .Select(match =>
            {
                var id = match.Groups[1].Value;
                var flow = int.Parse(match.Groups[2].ValueSpan);
                var tunnels = match.Groups[3].Value.Split(", ");
                return (id, flow, tunnels);
            })
            .OrderByDescending(v => v.flow)
            .ToList();


        // Construct valve objects
        var valves = new Valve[valveData.Count];
        for (var index = 0; index < valveData.Count; index++)
        {
            valves[index] = new Valve(valveData[index].id, valveData[index].flow, index, valveData.Count);
        }
        
        // Map tunnels phase 1 - list edges for Dijkstra 
        var valveEdges = new List<int>[valveData.Count];
        for (var index = 0; index < valveData.Count; index++)
        {
            valveEdges[index] = valveData[index].tunnels
                .Select(id =>
                {
                    var targetValve = Array.Find(valves, v => v.ID == id);
                    if (targetValve == null)
                    {
                        throw new ArgumentException($"Input contains a reference to non-existent valve: {id}");
                    }
                    return targetValve.ValveIndex;
                })
                .ToList();
        }

        // Map tunnels phase 2 - compute distances
        for (var fromIndex = 0; fromIndex < valveData.Count; fromIndex++)
        {
            // Compute paths from here to every other node
            var parents = FindShortestPathsTo(valves, valveEdges, fromIndex);
            var fromValve = valves[fromIndex];
            
            // Map the nodes.
            // Split the loops like this because the input graph is symmetric.
            // All tunnels are two-way.
            for (var toIndex = fromIndex + 1; toIndex < valveData.Count; toIndex++)
            {
                // This will be reversed, which happens to be what we want for "to" to "from".
                // This is also inclusive, so we need to skip the first element.
                var path = GetPath(parents, fromIndex, toIndex);
                var toValve = valves[toIndex];
                toValve.PathTo[fromIndex] = path.Skip(1).ToArray();
                
                // Now we "reverse" it back to the correct order.
                // Don't forget to skip the first!
                path.Reverse();
                fromValve.PathTo[toIndex] = path.Skip(1).ToArray();
            }
        }

        return valves;
    }
    
    // Return the path from start to root, INCLUDING both ends
    private static List<int> GetPath(int[] parents, int root, int start)
    {
        var path = new List<int>();
        for (var next = start; next != root; next = parents[next])
        {
            path.Add(next);
        }
        path.Add(root);
        return path;
    }

    // Use BFS to compute the shortest-path tree.
    private static int[] FindShortestPathsTo(Valve[] valves, List<int>[] valveEdges, int root)
    {
        var queue = new Queue<int>();
        var parents = new int[valves.Length];
        var explored = new bool[valves.Length];
        
        explored[root] = true;
        queue.Enqueue(root);

        while (queue.TryDequeue(out var from))
        {
            foreach (var to in valveEdges[from])
            {
                if (!explored[to])
                {
                    explored[to] = true;
                    parents[to] = from;
                    queue.Enqueue(to);
                }
            }
        }

        return parents;
    }
}

// Because writing "List<List<Move>>" gets REALLY old, REALLY fast.