using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Day16;

public class Path
{
    public bool CanContinue => TimeUsed < 30;
    public int TimeUsed { get; private set; }
    public int MinFlow { get; private set; }
    public int MaxFlow { get; private set; }
    public Valve Position { get; private set; }

    public IReadOnlyList<Move> Moves => _moves;
    private readonly List<Move> _moves = new();
    
    public bool[] ValveStatuses { get; }

    public Path(Valve startingPosition, int numValves)
    {
        Position = startingPosition;
        ValveStatuses = new bool[numValves];
    }

    private Path(Valve position, bool[] valveStatuses, int timeUsed, int minFlow, int maxFlow, List<Move> moves)
    {
        Position = position;
        ValveStatuses = valveStatuses;
        TimeUsed = timeUsed;
        MinFlow = minFlow;
        MaxFlow = maxFlow;
        _moves = moves;
    }

    public Path Clone()
    {
        var statusClone = new bool[ValveStatuses.Length];
        Array.Copy(ValveStatuses, statusClone, ValveStatuses.Length);

        var movesClone = new List<Move>(_moves);

        return new Path(Position, statusClone, TimeUsed, MinFlow, MaxFlow, movesClone);
    }

    public void Terminate()
    {
        TimeUsed = 30;
        MaxFlow = MinFlow;
    }

    public StringBuilder TracePath(Valve[] valves)
    {
        var sb = new StringBuilder();

        var openValves = new List<int>();

        void PrintOpenValves()
        {
            if (openValves.Count == 0)
            {
                sb.Append("No valves are open.\n");
                return;
            }

            if (openValves.Count == 1)
            {
                sb.Append("Valve ");
                sb.Append(valves[openValves[0]]);
                sb.Append(" is ");
            }
            else
            {
                sb.Append("Valves ");
                if (openValves.Count == 2)
                {
                    sb.Append(valves[openValves[0]]);
                    sb.Append(" and ");
                    sb.Append(valves[openValves[1]]);
                }
                else
                {
                    for (var i = 0; i < openValves.Count; i++)
                    {
                        if (i > 0)
                            sb.Append(", ");
                        if (i == openValves.Count - 1)
                            sb.Append("and ");
                        sb.Append(valves[openValves[i]]);
                    }
                }
                sb.Append(" are ");
            }

            sb.Append("open, releasing ");
            sb.Append(openValves.Aggregate(0, (sum, i) => sum + valves[i].FlowRate));
            sb.Append(" pressure.\n");
        }
        
        var minute = 0;
        foreach (var move in _moves)
        {
            minute++;
            sb.Append("== Minute ");
            sb.Append(minute);
            sb.Append(" ==\n");
            PrintOpenValves();
            sb.Append("You move to valve ");
            sb.Append(valves[move.ValveIndex].ID);
            sb.Append(".\n\n");

            if (move.OpenValve)
            {
                minute++;
                sb.Append("== Minute ");
                sb.Append(minute);
                sb.Append(" ==\n");
                PrintOpenValves();
                sb.Append("You open valve ");
                sb.Append(valves[move.ValveIndex].ID);
                sb.Append(".\n\n");
                
                openValves.Add(move.ValveIndex);
            }
        }
        
        return sb;
    }
    
    public void MakeMove(Valve[] valves, Move move)
    {
        Debug.Assert(TimeUsed < 30, "Cannot make a move - this path as out of time");
        Debug.Assert(Position.PathTo[move.ValveIndex].Length == 1, $"Cannot move from {Position} to {move.ValveIndex} - they are not connected");
        
        // Update path
        var valve = valves[move.ValveIndex];
        Position = valve;
        _moves.Add(move);
     
        // All movements take one minute
        TimeUsed++;
        
        // Open the valve if OpenValve flag is set
        if (move.OpenValve)
        {
            Debug.Assert(!ValveStatuses[move.ValveIndex], $"Cannot open valve {move.ValveIndex} because it is already opened");
            
            TimeUsed++; // It takes an extra minute to open the valve
            ValveStatuses[move.ValveIndex] = true; // Record valve as opened so we don't open it twice
            MinFlow += ComputeNetFlow(valve.FlowRate, TimeUsed); // Increase flow to account for new valve - AFTER time is updated!
        }
        
        // Update stats
        MaxFlow = ComputeMaxFlow(valves);
    }

    private int ComputeMaxFlow(Valve[] valves)
    {
        // Start with our current flow to present
        var maxFlow = MinFlow;
        
        // Add in the magically ideal case for remaining valves.
        // There is no possible way to get more flow than this, so it forms an upper bound for this path
        var nextTime = TimeUsed + 1;
        for (var index = 0; index < valves.Length ; index++)
        {
            // Stop if we run out of time
            if (nextTime >= 30)
                break;
                
            // Skip opened valves (they are already counted)
            if (ValveStatuses[index])
                continue;
            
            // Add the next valve
            maxFlow += ComputeNetFlow(valves[index].FlowRate, nextTime);
            nextTime++;
        }

        return maxFlow;
    }
    
    private static int ComputeNetFlow(int flowRate, int timeUsed) => (30 - timeUsed) * flowRate;
}