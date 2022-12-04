using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace AdventOfCode.Util;

public class SolutionConsoleFormatter : ConsoleFormatter
{

    public SolutionConsoleFormatter() : base(nameof(SolutionConsoleFormatter)) {}
    
    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);
        var category = GetCategoryName(logEntry.Category);
        textWriter.Write($"[{category}] ");
        textWriter.WriteLine(message);
    }
    private static string GetCategoryName(string logEntryCategory)
    {
        var split = logEntryCategory.LastIndexOf('.') + 1;
        if (split <= 0 || split >= logEntryCategory.Length)
        {
            return logEntryCategory;
        }

        return logEntryCategory.Substring(split);
    }
}