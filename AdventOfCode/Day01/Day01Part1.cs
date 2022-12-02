using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day01;

public class Day01Part1 : Day01
{
    protected override void RunPart(IEnumerable<int> calories)
    {
        var maxCalories = calories.Max();
        Log($"The max number of calories is [{maxCalories}].");
    }
} 