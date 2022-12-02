using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day01;

public static class Day01Part1
{
    public static async Task Run(string[] args)
    {
        var elves = await Day01.ParseInput();
        
        var elfWithMax = elves.MaxBy(elf => elf.Calories) ?? throw new ApplicationException("No elves were loaded from the input");
        
        Console.WriteLine($"Elf {elfWithMax.Number} has the most calories ({elfWithMax.Calories}).");
    }
} 