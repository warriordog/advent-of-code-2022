using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day01;

public class Day01Part1 : Day01
{
    private readonly ILogger<Day01Part1> _logger;
    public Day01Part1(ILogger<Day01Part1> logger) => _logger = logger;
    
    protected override void RunPart(IEnumerable<int> calories)
    {
        var maxCalories = calories.Max();
        _logger.LogInformation($"The max number of calories is [{maxCalories}].");
    }
} 