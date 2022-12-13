using AdventOfCode.Common;

namespace AdventOfCode.Day13;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
public abstract class Day13 : ISolution
{
    public void Run(string inputFile)
    {
        var inputLines = inputFile
            .AsSpan()
            .TrimEnd()
            .SplitLines();

        RunDay13(inputLines);
    }
    
    protected abstract void RunDay13(SpanLineSplitter inputLines);
}