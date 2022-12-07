using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day04;

[Solution("Day04", "Part2")]
[InputFile("input.txt", resolution: InputFileResolution.PathRelativeToSolution)]
public class Day04Part2 : Day04
{    
    private readonly ILogger<Day04Part2> _logger;
    public Day04Part2(ILogger<Day04Part2> logger) => _logger = logger;
    
    protected override void RunDay4(IEnumerable<Pair> pairs)
    {
        var numOverlapping = pairs.Count(pair =>
            (pair.Shorter.Max >= pair.Longer.Min && pair.Shorter.Max <= pair.Longer.Max) ||
            (pair.Shorter.Min >= pair.Longer.Min && pair.Shorter.Min <= pair.Longer.Max));

        _logger.LogInformation("There are [{numOverlapping}] partially overlapping pairs.", numOverlapping);
    }
}