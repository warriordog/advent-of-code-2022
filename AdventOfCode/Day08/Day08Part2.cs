using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day08;

[Solution("Day08", "Part2")]
[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
[InputFile("test2.txt", InputFileType.Test)]
public class Day08Part2 : Day08
{
 
    private readonly ILogger<Day08Part2> _logger;
    public Day08Part2(ILogger<Day08Part2> logger) => _logger = logger;
    
    protected override void RunDay8(ReadOnlySpan<char> input, int gridSize, int rowSkip)
    {
        var maxScenic = 0;
    
        // Brute force this thing:    
        // Check each tree against each other tree in O(n^2) worst-case time!
        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                var scenicValue = ComputeScenicValue(row, col, input, rowSkip, gridSize);
                if (scenicValue > maxScenic)
                {
                    maxScenic = scenicValue;
                }
            }
        }
        
        _logger.LogInformation("The maximum scenic value in [{maxScenic}]", maxScenic);
    }

    private static int ComputeScenicValue(int row, int col, ReadOnlySpan<char> input, int rowSkip, int gridSize)
    {
        var tree = ReadTree(row, col, input, rowSkip);

        // MULTIPLY, not add!!
        return
            // Top
            CountVisibleTreesFrom(tree, row, col, 0, -1, input, rowSkip, gridSize) *

            // Right
            CountVisibleTreesFrom(tree, row, col, 1, 0, input, rowSkip, gridSize) *

            // Bottom
            CountVisibleTreesFrom(tree, row, col, 0, 1, input, rowSkip, gridSize) *

            // Left
            CountVisibleTreesFrom(tree, row, col, -1, 0, input, rowSkip, gridSize);
    }

    // Lets stress-test the stack:
    // 38 bytes passed on every method call
    // * 4 method calls for every tree
    // * 10000 trees in the input file
    // == 1520000 bytes passed in total!
    // At least its all on the stack, no heap allocations here
    private static int CountVisibleTreesFrom(char fromTree, int row, int col, int rowIncrement, int colIncrement, ReadOnlySpan<char> input, int rowSkip, int gridSize)
    {
        var count = 0;

        do
        {
            row += rowIncrement;
            col += colIncrement;

            // Need to break early to avoid counting outside the edges
            if (row < 0 || row >= gridSize || col < 0 || col >= gridSize)
            {
                break;
            }
            
            count++;
        } while (ReadTree(row, col, input, rowSkip) < fromTree);

        return count;
    }
}