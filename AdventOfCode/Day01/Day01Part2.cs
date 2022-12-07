using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day01;

[Solution("Day01", "Part2")]
public class Day01Part2 : Day01
{
    
    private readonly ILogger<Day01Part2> _logger;
    public Day01Part2(ILogger<Day01Part2> logger) => _logger = logger;
    
    protected override void RunPart(IEnumerable<int> calories)
    {
        var max3Calories = calories
            .OrderByDescending(c => c)
            .Take(3)
            .Aggregate(0, (max, c) => max + c);
        
        _logger.LogInformation($"The top 3 elves have a total of [{max3Calories}] calories.");
    }
}