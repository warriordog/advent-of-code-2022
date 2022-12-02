using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day01;

public class Day01Part1 : Day01
{
    protected override void RunPart(IEnumerable<Elf> elves)
    {
        var elfWithMax = elves.MaxBy(elf => elf.Calories) ?? throw new ApplicationException("No elves were loaded from the input");
        
        Console.WriteLine($"Elf {elfWithMax.Number} has the most calories ({elfWithMax.Calories}).");
    }
} 