using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day07;

[Solution("Day07", "Part2")]
[InputFile("input.txt")]
[InputFile("test.txt", type: InputFileType.Test)]
public class Day07Part2 : Day07
{
    private readonly ILogger<Day07Part2> _logger;
    public Day07Part2(ILogger<Day07Part2> logger) => _logger = logger;

    protected override void RunDay7(Directory rootDirectory)
    {
        const ulong MaxSpace = 70000000ul;
        const ulong MinSpace = 30000000ul;

        var usedSpace = rootDirectory.GetTotalSize();
        var freeSpace = MaxSpace - usedSpace;
        var targetSpace = MinSpace - freeSpace;
        
        _logger.LogDebug("Filesystem is using {used} / {total} bytes, leaving {free} bytes free.", usedSpace, MaxSpace, freeSpace);
        _logger.LogDebug("We need to free at least {target} bytes to reach the minimum of {min} bytes free.", targetSpace, MinSpace);

        var sizeToRemove = rootDirectory.AllDirectories
            .Select(d => d.GetTotalSize())
            .Where(size => size >= targetSpace)
            .Min();
        _logger.LogInformation("We need to delete a directory with [{size}] bytes to free up enough space.", sizeToRemove);
    }
}