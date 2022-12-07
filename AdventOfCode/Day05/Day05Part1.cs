using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day05;

[Solution("Day05", "Part1")]
[InputFile("input.txt")]
[InputFile("test.txt", type: InputFileType.Test)]
public class Day05Part1 : Day05<Stack<char>>
{
    public Day05Part1(ILogger<Day05Part1> logger): base(logger) {}


    protected override void AddToStack(Stack<char> stack, char crate) => stack.Push(crate);
    
    protected override void ExecuteMove(Stack<char> from, Stack<char> to, byte count)
    {
        for (var i = 0; i < count; i++)
        {
            var crate = from.Pop();
            to.Push(crate);
        }
    }
    
    protected override IEnumerable<char> GetTopOfStacks(IEnumerable<Stack<char>> stacks) => stacks.Select(s => s.TryPeek(out var c) ? c : ' ');
}