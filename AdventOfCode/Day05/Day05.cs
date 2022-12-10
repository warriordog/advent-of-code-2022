using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day05;

[InputFile("input.txt")]
[InputFile("test.txt", type: InputFileType.Test)]
public abstract class Day05<TStack> : ISolution
    where TStack : new()
{
    private readonly ILogger _logger;
    protected Day05(ILogger logger) => _logger = logger;
    
    public void Run(string inputFile)
    {
        // Parse everything
        var (stackSection, movesSection) = SplitInputFile(inputFile);
        var stacks = ParseStacks(stackSection);
        var moves = ParseMoves(movesSection);

        // Run it
        ExecuteMoves(stacks, moves);

        // Print results
        var stackTops = GetTopOfStacks(stacks);
        var stackTopString = string.Concat(stackTops);
        _logger.LogInformation("The crates on top of the stacks are: [{stackTopString}]", stackTopString);
    }
    
    private void ExecuteMoves(TStack[] stacks, IEnumerable<Move> moves)
    {
        foreach (var move in moves)
        {
            var fromStack = stacks[move.From];
            var toStack = stacks[move.To];

            ExecuteMove(fromStack, toStack, move.Count);
        }
    }

    /// <summary>
    /// Adds the specified crate onto a stack
    /// </summary>
    /// <param name="stack"></param>
    /// <param name="crate"></param>
    protected abstract void AddToStack(TStack stack, char crate);
    
    /// <summary>
    /// Moves a number of crates from one stack to another.
    /// Order is implementation-defined.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="count"></param>
    protected abstract void ExecuteMove(TStack from, TStack to, byte count);
    
    /// <summary>
    /// Get the top-most crate from a list of stacks
    /// </summary>
    /// <param name="stacks"></param>
    /// <returns></returns>
    protected abstract IEnumerable<char> GetTopOfStacks(IEnumerable<TStack> stacks);

    private (IEnumerable<string>, IEnumerable<string>) SplitInputFile(string inputFile)
    {
        // Find split point and cache details
        int splitLength;
        string newlineToken;
        var split = inputFile.IndexOf("\r\n\r\n", StringComparison.Ordinal);
        if (split >= 0)
        {
            splitLength = 4;
            newlineToken = "\r\n";
        }
        else
        {
            split = inputFile.IndexOf("\n\n", StringComparison.Ordinal);
            splitLength = 2;
            newlineToken = "\n";
        }
        var hasTrailingNewline = inputFile.EndsWith(newlineToken, StringComparison.Ordinal);
        
        // Validate input and findings
        if (split < 0)
            throw new ArgumentException("Input file is in the wrong format - cannot find two consecutive newlines", nameof(inputFile));
        if (split == 0)
            throw new ArgumentException("Input file is in the wrong format - stack section is empty", nameof(inputFile));
        if (split + splitLength >= inputFile.Length)
            throw new ArgumentException("Input file is in the wrong format - moves section is empty", nameof(inputFile));

        // Get sections
        // var stackSection = inputFile[..split]
        //     .Split(splitTokens, StringSplitOptions.RemoveEmptyEntries);
        // var movesSection = inputFile[(split + splitLength)..]
        //     .Split(splitTokens, StringSplitOptions.RemoveEmptyEntries);
        var stackSection = inputFile[..split]
            .SplitLazyReverse(newlineToken);
        var movesSection = inputFile[(split + splitLength)..]
            .SplitLazy(newlineToken);

        if (hasTrailingNewline)
        {
            movesSection = movesSection.SkipLast(1);
        }

        return (stackSection, movesSection);
    }
    
    private TStack[] ParseStacks(IEnumerable<string> section)
    {
        var stacks = Array.Empty<TStack>();
        var numStacks = stacks.Length;

        var isFirstLine = true;
        foreach (var line in section)
        {
            // This runs bottom-up, so the first line is actually the labels.
            // We skip it, but not before using it to calculate how many stacks we need.
            if (isFirstLine)
            {
                // Little bitty hack to skip parsing the footer row:
                // We only care about the number of columns because they're all in order.
                // Each column is exactly 3 characters wide w/ one spacer, so we can use math to find the number.
                // The last column has no trailing spacer, so add one to the length.
                // Now we can pretend that each column is 4 characters wide with no spacer, so divide by 4 to get the number of columns.
                numStacks = (line.Length + 1) / 4;

                // Initialize enough stacks to store the crates
                stacks = new TStack[numStacks];
                for (var i = 0; i < numStacks; i++)
                {
                    stacks[i] = new TStack();
                }

                // Skip forward to the first "real" line
                isFirstLine = false;
                continue;
            }

            // Load each crate into correct column.
            for (var stackIdx = 0; stackIdx < numStacks; stackIdx++)
            {
                // Starting with index 1, every 4th character is a crate or empty space
                var crateIdx = 1 + (stackIdx * 4);
                var crate = line[crateIdx];
                
                // Empty space means no crate
                if (crate != ' ') {
                    AddToStack(stacks[stackIdx], crate);
                }
            }
        }

        return stacks;
    }
    
    private static IEnumerable<Move> ParseMoves(IEnumerable<string> section)
    {
        foreach (var line in section)
        {
            SplitMoveLine(line, out var fromStr, out var toStr, out var countStr);

            var from = FastParseStack(fromStr);
            var to = FastParseStack(toStr);
            var count = byte.Parse(countStr);

            // Yield return - we don't need an intermediate storage
            yield return new Move(from, to, count);
        }
    }

    private static void SplitMoveLine(string line, out string from, out string to, out string count)
    {
        var start = 5; // Skip over "move "
        start = ExtractUntilSpace(line, start, out count);
        start += 5; // Skip over "from ";
        start = ExtractUntilSpace(line, start, out from);
        start += 3; // Skip over "to ";
        to = line[start..];
    }

    private static int ExtractUntilSpace(string line, int start, out string extracted)
    {
        var end = line.IndexOf(' ', start);
        extracted = line[start..end]; // Extract the word
        return end + 1; // Consume the space and return next starting point
    }

    private static byte FastParseStack(string stack)
    {
        if (stack.Length == 1)
        {
            return stack[0] switch
            {
                // Not a typo - we are bundling the conversion from 1-indexed to 0-indexed numbering to remove a subtraction
                '1' => 0,
                '2' => 1,
                '3' => 2,
                '4' => 3,
                '5' => 4,
                '6' => 5,
                '7' => 6,
                '8' => 7,
                '9' => 8,
                '0' => throw new ArgumentOutOfRangeException(nameof(stack), "Stack ID must be greater than 0"),
                _ => throw new ArgumentOutOfRangeException(nameof(stack), "Stack ID must be a number")
            };
        }

        // Ugly casting is required by C# bc it could underflow
        return (byte)(byte.Parse(stack) - 1);
    }
}

/// <summary>
/// A movement operation for the giant cargo crane.
/// </summary>
/// <remarks>
/// All values are bytes so that the entire structure can fit within a 32-bit word.
/// This is fine because official inputs don't go very high.
/// </remarks>
/// <param name="From">Index of the stack to move from</param>
/// <param name="To">Index of the stack to move to</param>
/// <param name="Count">Number of crates to move</param>
public readonly record struct Move(byte From, byte To, byte Count);