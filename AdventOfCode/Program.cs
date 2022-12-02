using AdventOfCode.Common;
using AdventOfCode.Day01;

namespace AdventOfCode;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var (dayName, partName) = args.Parse("day name/number", "part name/number");
        var solutionArgs = args.Skip(2).ToList();
        var solutionName = $"AdventOfCode.{dayName}.{dayName}{partName}";
        var solutionFile = $"AdventOfCode/{dayName}/input.txt";
        
        var solution = LoadSolution(solutionName);
        
        Console.WriteLine($"Running {solutionName}.");
        await solution.Run(solutionFile, solutionArgs);
    }

    private static ISolution LoadSolution(string solutionTypeName)
    {
        var type = Type.GetType(solutionTypeName);
        if (type == null)
        {
            Console.Error.WriteLine($"No solution found with name {solutionTypeName}");
            throw new ArgumentException("Unable to load type for name", nameof(solutionTypeName));
        }

        if (!type.IsAssignableTo(typeof(ISolution)))
        {
            Console.Error.WriteLine($"No solution found with name {solutionTypeName}");
            throw new ArgumentException("Type does not implement ISolution", nameof(solutionTypeName));
        }

        var obj = Activator.CreateInstance(type);
        if (obj is not ISolution solution)
        {
            Console.Error.WriteLine($"No solution found with name {solutionTypeName}");
            throw new ApplicationException($"Could not create an instance of type {type.FullName}");
        }

        return solution;
    }
}