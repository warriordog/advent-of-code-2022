using System.Text.RegularExpressions;

namespace AdventOfCode.Common;

public static class StringExtensions
{
    private static readonly Regex EOLRegex = new("\r?\n", RegexOptions.Compiled);
    private static readonly Regex TwoEOLRegex = new("\r?\n\r?\n", RegexOptions.Compiled);
    
    public static string[] SplitByTwoEOL(this string str) => TwoEOLRegex.Split(str);
    public static string[][] SplitByTwoThenOneEOL(this string str) => SplitByTwoEOL(str)
        .Select(chunk => EOLRegex.Split(chunk))
        .ToArray();

    public static string[] SplitByEOL(this string str) => EOLRegex.Split(str);

    public static IEnumerable<string> SkipEmptyStrings(this IEnumerable<string> strings) => strings.Where(str => str.Length > 0);
}