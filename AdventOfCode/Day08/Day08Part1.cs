using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day08;

[Solution("Day08", "Part1")]
public class Day08Part1 : Day08
{
    private readonly ILogger<Day08Part1> _logger;
    public Day08Part1(ILogger<Day08Part1> logger) => _logger = logger;
    
    protected override void RunDay8(ReadOnlySpan<char> input, int gridSize, int rowSkip)
    {
        // Pre-add the outer ring.
        // Have to subtract 1 or the corners will be counted twice!
        var accessible = (gridSize - 1) * 4;

        // Test each cell in the grid
        for (var row = 1; row < gridSize - 1; row++)
        {
            for (var col = 1; col < gridSize - 1; col++)
            {
                // Get the target tree to compare
                var tree = ReadTree(row, col, input, rowSkip);

                if (
                    // Check top
                    CheckTop(row, col, tree, input, rowSkip) ||
                    // Check right
                    CheckRight(row, col, tree, input, rowSkip, gridSize) ||
                    // Check bottom
                    CheckBottom(row, col, tree, input, rowSkip, gridSize) ||
                    // Check left
                    CheckLeft(row, col, tree, input, rowSkip)
                )
                {
                    accessible++;
                }
            }
        }
        
        _logger.LogInformation("There are [{numTrees}] trees visible from the outside.", accessible);
    }

    private bool CheckTop(int row, int startCol, char tree, ReadOnlySpan<char> input, int rowSkip)
    {
        for (var col = startCol - 1; col >= 0; col--)
        {
            if (ReadTree(row, col, input, rowSkip) >= tree)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckRight(int startRow, int col, char tree, ReadOnlySpan<char> input, int rowSkip, int gridSize)
    {
        for (var row = startRow + 1; row < gridSize; row++)
        {
            if (ReadTree(row, col, input, rowSkip) >= tree)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckBottom(int row, int startCol, char tree, ReadOnlySpan<char> input, int rowSkip, int gridSize)
    {
        for (var col = startCol + 1; col < gridSize; col++)
        {
            if (ReadTree(row, col, input, rowSkip) >= tree)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckLeft(int startRow, int col, char tree, ReadOnlySpan<char> input, int rowSkip)
    {
        for (var row = startRow - 1; row >= 0; row--)
        {
            if (ReadTree(row, col, input, rowSkip) >= tree)
            {
                return false;
            }
        }

        return true;
    }
}