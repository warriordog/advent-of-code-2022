using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day03;

[Solution("Day03", "Part2")]
public class Day03Part2 : Day03
{
    private readonly ILogger<Day03Part2> _logger;
    public Day03Part2(ILogger<Day03Part2> logger) => _logger = logger;

    protected override void RunDay3(IEnumerable<int[][][]> groups)
    {
        var badgePrioritySum = groups
            .Aggregate(0, (sum, group) => sum + AllPriorities
                .First(priority => group
                    .All(sack => sack
                        .Any(comp => comp[priority] > 0)
                    )
                )
            );
        
        _logger.LogInformation($"The total priority of all badges types is [{badgePrioritySum}].");
    }
}