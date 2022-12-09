using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day09;

[Solution("Day09", "Part2")]
[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
[InputFile("challenge_1m.txt", InputFileType.Challenge)]
[InputFile("challenge_10m.txt", InputFileType.Challenge)]
public class Day09Part2 : Day09
{
    public Day09Part2(ILogger<Day09Part2> logger)
        : base(logger, 10)
    {}
}