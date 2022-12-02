namespace AdventOfCode;

public interface ISolution
{
    public Task Run(string inputFile, List<string> solutionArgs);
}