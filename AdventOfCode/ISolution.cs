namespace AdventOfCode;

/// <summary>
/// An independently-executable solution to a day/part pair.
/// Implementations must have a public no-args constructor.
/// </summary>
public interface ISolution
{
    /// <summary>
    /// Execute the solution.
    /// Implementations MAY be asynchronous.
    /// </summary>
    /// <param name="inputFile">Relative path to the input file to use</param>
    /// <param name="solutionArgs">Additional command line arguments provided by the user</param>
    /// <returns>Return a task to await, even if the solution is synchronous</returns>
    public Task Run(string inputFile, List<string> solutionArgs);
}