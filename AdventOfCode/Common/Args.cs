namespace AdventOfCode.Common;

public static class Args
{
    private static string GetArg(IReadOnlyList<string> args, string name, int index)
    {
        if (args.Count <= index)
        {
            Console.Error.WriteLine($"Missing required argument #{index + 1} - {name}");
            Environment.Exit(-1);
        }

        return args[index];
    }
    
    public static string Parse(this IReadOnlyList<string> args, string argName) => Parse(args, argName, a => a);
    public static T Parse<T>(this IReadOnlyList<string> args, string argName, Func<string, T> argConverter)
    {
        var argValue = GetArg(args, argName, 0);
        return argConverter(argValue);
    }
    
    public static (string, string) Parse(this IReadOnlyList<string> args, string arg1Name, string arg2Name) => Parse(args, arg1Name,  a => a, arg2Name, a => a);
    public static (T1, T2) Parse<T1, T2>(this IReadOnlyList<string> args, string arg1Name, Func<string, T1> arg1Converter, string arg2Name, Func<string, T2> arg2Converter)
    {
        var arg1Value = GetArg(args, arg1Name, 0);
        var arg2Value = GetArg(args, arg2Name, 1);
        return (
            arg1Converter(arg1Value),
            arg2Converter(arg2Value)
        );
    }
}