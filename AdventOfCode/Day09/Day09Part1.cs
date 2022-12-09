using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day09;

[Solution("Day09", "Part1")]
[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
[InputFile("challenge_1m.txt", InputFileType.Challenge)]
[InputFile("challenge_10m.txt", InputFileType.Challenge)]
public class Day09Part1 : Day09
{
    public Day09Part1(ILogger<Day09Part1> logger)
        : base(logger, 2)
    {}
}