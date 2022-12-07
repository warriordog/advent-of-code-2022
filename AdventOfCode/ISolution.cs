namespace AdventOfCode;

/// <summary>
/// An executable AdventOfCode solution.
/// Solutions must have a public constructor.
/// Constructor arguments are allowed and will be populated from the runner's DI container.
/// </summary>
public interface ISolution
{
    /// <summary>
    /// Execute the solution.
    /// </summary>
    /// <param name="inputFile">Content of the input file</param>
    public void Run(string inputFile);
}