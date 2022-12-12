namespace AdventOfCode.Day11;

public class Monkey
{
    public int Id { get; }
    public Func<ulong, ulong> Operation { get; }
    public ulong TestDivisor { get; }
    
    public Queue<ulong> Items { get; }
    public int TrueMonkey { get; }
    public int FalseMonkey { get; }
    
    public ulong InspectionCount { get; set; }
    
    public Monkey(int id, Func<ulong, ulong> operation, ulong testDivisor, int trueMonkey, int falseMonkey, Queue<ulong> items)
    {
        Id = id;
        Operation = operation;
        TestDivisor = testDivisor;
        TrueMonkey = trueMonkey;
        FalseMonkey = falseMonkey;
        Items = items;
    }
}