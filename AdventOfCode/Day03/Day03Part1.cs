using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day03;

[Solution("Day03", "Part1")]
public class Day03Part1 : Day03
{
    private readonly ILogger<Day03Part1> _logger;
    public Day03Part1(ILogger<Day03Part1> logger) => _logger = logger;

    protected override void RunDay3(IEnumerable<int[][][]> groups)
    {
        var conflictPrioritySum = groups
            .SelectMany(group => group)
            .Aggregate(0, (sum, sack) => sum + AllPriorities
                .First(priority => sack
                    .All(comp => comp[priority] > 0)
                )
            );
        
        _logger.LogInformation($"The total priority of conflicting items is [{conflictPrioritySum}].");
    }
}