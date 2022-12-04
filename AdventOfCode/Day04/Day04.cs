using System.Text.RegularExpressions;

namespace AdventOfCode.Day04;

public abstract class Day04 : ISolution
{
    // TIL that carriage return is not part of the line terminator, at least for .NET Regex
    // https://stackoverflow.com/questions/8618557/why-doesnt-in-net-multiline-regular-expressions-match-crlf
    private static readonly Regex ParsePairsRegex = new(@"^(\d+)-(\d+),(\d+)-(\d+)\r?$", RegexOptions.Compiled | RegexOptions.Multiline);

    public void Run(string inputFile)
    {
        var pairs = ParsePairsRegex
            .Matches(inputFile)
            .Select(match => ParsePair(match.Groups));

        RunDay4(pairs);
    }

    protected abstract void RunDay4(IEnumerable<Pair> pairs);

    private static Pair ParsePair(GroupCollection matchGroup)
    {
        var first = ParseAssignment(matchGroup, 1);
        var firstLength = first.Max - first.Min;
        
        var second = ParseAssignment(matchGroup, 3);
        var secondLength = second.Max - second.Min;

        // Pre-sort the assignment ranges to simplify later logic
        var firstIsLarger = firstLength >= secondLength;
        var longer = firstIsLarger ? first : second;
        var shorter = firstIsLarger ? second : first;
        
        return new Pair(longer, shorter);
    }

    private static Assignment ParseAssignment(GroupCollection matchGroup, int startIdx)
    {
        var min = byte.Parse(matchGroup[startIdx].Value);
        var max = byte.Parse(matchGroup[startIdx + 1].Value);
        return new Assignment(min, max);
    }
}

/// <summary>
/// A pair of assignments in the same general area.
/// Assignments are sorted based on the size of the work area.
/// </summary>
/// <param name="Longer">The larger / longer of the two assignments</param>
/// <param name="Shorter">The smaller / shorter of the two assignments</param>
public readonly record struct Pair(Assignment Longer, Assignment Shorter);

/// <summary>
/// A work assignment for an elf.
/// All sections between Min and Max (inclusively) are considered to be part of the assignment.
/// </summary>
/// <param name="Min">Lower bound (inclusive) of the work area</param>
/// <param name="Max">Upper bound (inclusive) of the work area</param>
public readonly record struct Assignment(byte Min, byte Max);