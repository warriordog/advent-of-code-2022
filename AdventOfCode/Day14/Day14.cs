using System.Text;
using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day14;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
[InputFile("test2.txt", InputFileType.Test)]
public abstract class Day14<TData> : ISolution
{
    private readonly ILogger _logger;
    protected Day14(ILogger logger) => _logger = logger;

    public void Run(string inputFile)
    {
        var input = inputFile
            .AsSpan()
            .TrimEnd()
            .SplitLines()
        ;
        
        var scanData = ParseInput(input);
        _logger.LogDebug("Before simulation, the cave looks like:\n{cave}", scanData);
        
        var sandCount = RunSimulation(scanData);

        _logger.LogDebug("After simulation, the cave looks like:\n{cave}", scanData);
        _logger.LogInformation("A total of [{sandCount}] grains of sand came to rest.", sandCount);
    }
    

    protected abstract TData ParseInput(SpanLineSplitter input);
    protected abstract int RunSimulation(TData scanData);
}

public enum Matter
{
    Air = 0, // Air should be zero to support some optimizations
    Void = 1,
    Rock = 2,
    Sand = 3
}

// Please Mr. Microsoft, can we have Java-style enums? Pretty please?
public static class MatterExtensions
{
    public static char GetDisplayChar(this Matter matter) => matter switch
    {
        Matter.Void => '@',
        Matter.Air => ' ',
        Matter.Rock => '█',
        Matter.Sand => 'o',
        _ => '?'
    };
}