namespace AdventOfCode.Day01;

public static class Day01Part2
{
    public static async Task Run(string[] args)
    {
        var elves = await Day01.ParseInput();

        var max3Calories = elves
            .OrderByDescending(elf => elf.Calories)
            .Take(3)
            .Aggregate(0, (max, elf) => max + elf.Calories);
        
     
        Console.WriteLine($"The top 3 elves have a total of {max3Calories} calories.");   
    }
}