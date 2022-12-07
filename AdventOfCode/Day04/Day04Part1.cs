using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day04;

[Solution("Day04", "Part1")]
[InputFile("input.txt")]
public class Day04Part1 : Day04
{
    
    private readonly ILogger<Day04Part1> _logger;
    public Day04Part1(ILogger<Day04Part1> logger) => _logger = logger;
    
    protected override void RunDay4(IEnumerable<Pair> pairs)
    {
        var numOverlapping = pairs.Count(pair =>
            pair.Shorter.Min >= pair.Longer.Min &&
            pair.Shorter.Max <= pair.Longer.Max);

        _logger.LogInformation("There are [{numOverlapping}] fully overlapping pairs.", numOverlapping);
    }
}