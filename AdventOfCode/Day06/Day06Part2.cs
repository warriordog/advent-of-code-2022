using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day06;

[Solution("Day06", "Part2")]
[InputFile("input.txt")]
public class Day06Part2 : Day06
{
    public Day06Part2(ILogger<Day06Part2> logger) : base(logger) {}
    
    protected override void RunDay6(ReadOnlySpan<char> dataStream)
    {
        var headerStart = FindUniqueSequence(dataStream, 14);
        var toSkip = headerStart + 14;
        Logger.LogInformation("The communicator must process [{toSkip}] characters before the first 14-character start-of-message marker is complete.", toSkip);
    }
}