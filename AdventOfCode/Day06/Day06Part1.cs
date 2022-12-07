using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day06;

[Solution("Day06", "Part1")]
[InputFile("input.txt")]
public class Day06Part1 : Day06
{

    public Day06Part1(ILogger<Day06Part1> logger) : base(logger) {}
    
    protected override void RunDay6(ReadOnlySpan<char> dataStream)
    {
        var headerStart = FindUniqueSequence(dataStream, 4);
        var toSkip = headerStart + 4;
        Logger.LogInformation("The communicator must process [{toSkip}] characters before the first 4-character start-of-packet marker is complete.", toSkip);
    }
}