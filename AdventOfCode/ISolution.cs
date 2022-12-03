namespace AdventOfCode;

/// <summary>
/// An independently-executable solution to a day/part pair.
/// Implementations must have a public no-args constructor.
/// </summary>
public interface ISolution
{
    /// <summary>
    /// Execute the solution.
    /// </summary>
    /// <param name="inputFile">Content of the input file</param>
    public void Run(string inputFile);
}