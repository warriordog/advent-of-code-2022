using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day05;

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

    private (string[], string[]) SplitInputFile(string inputFile)
    {
        // Find split point and cache details
        var splitLength = 4;
        var splitTokens = new[] { '\r', '\n' };
        var split = inputFile.IndexOf("\r\n\r\n", StringComparison.Ordinal);
        if (split < 0)
        {
            split = inputFile.IndexOf("\n\n", StringComparison.Ordinal);
            splitLength = 2;
            splitTokens = new[] { '\n' };
        }
        
        // Validate input and findings
        if (split < 0)
            throw new ArgumentException("Input file is in the wrong format - cannot find two consecutive newlines", nameof(inputFile));
        if (split == 0)
            throw new ArgumentException("Input file is in the wrong format - stack section is empty", nameof(inputFile));
        if (split + splitLength >= inputFile.Length)
            throw new ArgumentException("Input file is in the wrong format - moves section is empty", nameof(inputFile));

        // Get sections
        var stackSection = inputFile[..split]
            .Split(splitTokens, StringSplitOptions.RemoveEmptyEntries);
        var movesSection = inputFile[(split + splitLength)..]
            .Split(splitTokens, StringSplitOptions.RemoveEmptyEntries);

        return (stackSection, movesSection);
    }
    
    private TStack[] ParseStacks(string[] section)
    {
        // Little bitty hack to skip parsing the footer row:
        // We only care about the number of columns because they're all in order.
        // Each column is exactly 3 characters wide w/ one spacer, so we can use math to find the number.
        // The last column has no trailing spacer, so add one to the length.
        // Now we can pretend that each column is 4 characters wide with no spacer, so divide by 4 to get the number of columns.
        var numStacks = (section[0].Length + 1) / 4;

        // Initialize enough stacks to store the crates
        var stacks = new TStack[numStacks];
        for (var i = 0; i < numStacks; i++)
        {
            stacks[i] = new TStack();
        }
        
        // Parse the starting position of all stacks.
        // Work row-by-row from the bottom up so that lower indexes will represent lower positions in the stack.
        // Skip the last (highest index) row because that is the footer.
        for (var lineIdx = section.Length - 2; lineIdx >= 0; lineIdx--)
        {
            // Get row
            var row = section[lineIdx];
            
            // Load each crate into correct column.
            for (var stackIdx = 0; stackIdx < numStacks; stackIdx++)
            {
                // Starting with index 1, every 4th character is a crate or empty space
                var crateIdx = 1 + (stackIdx * 4);
                var crate = row[crateIdx];
                
                // Empty space means no crate
                if (crate != ' ') {
                    AddToStack(stacks[stackIdx], crate);
                }
            }
        }

        return stacks;
    }
    
    private static IEnumerable<Move> ParseMoves(string[] section)
    {
        for (var lineIdx = 0; lineIdx < section.Length; lineIdx++)
        {
            var line = section[lineIdx];

            // Skip over trailing newline
            if (lineIdx == section.Length - 1 && line.Length == 0)
                continue;

            // Another little magic trick - the only spaces are between tokens.
            // Each line has a consistent layout, so we can hardcode indexes after that.
            var parts = line.Split(" ");

            // Parse the values.
            // Sub 1 from "to" and "from" because they are 1-indexed but we are using 0-indexed arrays.
            // Ugly casting bc it "could" underflow
            var from = (byte)(byte.Parse(parts[3]) - 1);
            var to = (byte)(byte.Parse(parts[5]) - 1);
            var count = byte.Parse(parts[1]);

            // Yield return - we don't need an intermediate list
            yield return new Move(from, to, count);
        }
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