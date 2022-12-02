namespace AdventOfCode.Day01;

public class Day01Part2 : Day01
{
    protected override void RunPart(IEnumerable<int> calories)
    {
        var max3Calories = calories
            .OrderByDescending(c => c)
            .Take(3)
            .Aggregate(0, (max, c) => max + c);
        
        Log($"The top 3 elves have a total of [{max3Calories}] calories.");
    }
}