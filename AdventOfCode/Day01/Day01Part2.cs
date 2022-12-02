namespace AdventOfCode.Day01;

public class Day01Part2 : Day01
{
    protected override void RunPart(IEnumerable<Elf> elves)
    {
        var max3Calories = elves
            .OrderByDescending(elf => elf.Calories)
            .Take(3)
            .Aggregate(0, (max, elf) => max + elf.Calories);
        
     
        Console.WriteLine($"The top 3 elves have a total of {max3Calories} calories.");   
    }
}