using AdventOfCode.Common;
using AdventOfCode.Day01;

namespace AdventOfCode;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var (day, part) = args.Parse(
            "day number", int.Parse,
            "part number or name", a => a
            );
        
        Console.WriteLine($"Running day {day} part {part}.");
        await RunDayPart(day, part, args);
    }

    private static async Task RunDayPart(int day, string part, string[] args)
    {
        if (day == 1) await RunDay1(part, args);
        else UnknownDay(day);
    }

    private static async Task RunDay1(string part, string[] args)
    {
        if (part == "1") await Day01Part1.Run(args);
        else if (part == "2") await Day01Part2.Run(args);
        else UnknownPart(1, part);
    }

    private static void UnknownDay(int day)
    {
        Console.Error.WriteLine($"Unknown day number: {day}");
    }

    private static void UnknownPart(int day, string part)
    {
        Console.Error.WriteLine($"Unknown part \"{part}\" for day {day}");
    }
}