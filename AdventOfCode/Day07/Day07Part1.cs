using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day07;

[Solution("Day07", "Part1")]
[InputFile("input.txt")]
[InputFile("test.txt", type: InputFileType.Test)]
public class Day07Part1 : Day07
{
    private readonly ILogger<Day07Part1> _logger;
    public Day07Part1(ILogger<Day07Part1> logger) => _logger = logger;
    
    protected override void RunDay7(Directory rootDirectory)
    {
        _logger.LogDebug("Filesystem:\n{filesystem}", rootDirectory.DumpFilesystem());

        var candidates = rootDirectory
            .AllDirectories
            .Where(d => d.GetTotalSize() <= 100000)
            .ToList();

        var candidateSum = candidates
            .Aggregate(0ul, (sum, dir) => sum + dir.GetTotalSize());

        var candidateNames = string
            .Join("\n- ", candidates
                .Select(dir => dir.FullName)
                .OrderBy(name => name));
        
        _logger.LogDebug("Found {numCandidates} candidates:\n- {candidates}", candidates.Count, candidateNames);
        _logger.LogInformation("The total size of all candidate directories is [{candidateSum}].", candidateSum);
        
        // testing something
        Thread.Sleep(4000);
    }
}