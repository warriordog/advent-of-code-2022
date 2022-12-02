using System.Text.RegularExpressions;

namespace AdventOfCode.Common;

public static class StringExtensions
{
    private static readonly Regex EOLRegex = new("\r?\n");
    private static readonly Regex TwoEOLRegex = new("\r?\n\r?\n");
    
    public static string[] SplitByTwoEOL(this string str) => TwoEOLRegex.Split(str);
    public static string[][] SplitByTwoThenOneEOL(this string str) => SplitByTwoEOL(str)
        .Select(chunk => EOLRegex.Split(chunk))
        .ToArray();
}