namespace AdventOfCode.Day16;

public class Valve
{
    public int ValveIndex { get; }
    public string ID { get; }
    public int FlowRate { get; }
    public int[][] PathTo { get; }

    public Valve(string id, int flowRate, int valveIndex, int numValves)
    {
        ID = id;
        FlowRate = flowRate;
        ValveIndex = valveIndex;
        PathTo = new int[numValves][];
    }

    public override string ToString() => $"{ValveIndex}({ID})";
}