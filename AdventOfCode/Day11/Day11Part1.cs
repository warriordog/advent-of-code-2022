using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day11;

[Solution("Day11", "Part1")]
public class Day11Part1 : Day11
{
    public Day11Part1(ILogger<Day11Part1> logger)
        : base(logger, 20, 3ul)
    {}
}