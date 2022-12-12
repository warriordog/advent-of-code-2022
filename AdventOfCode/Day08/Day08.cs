using AdventOfCode.Common;

namespace AdventOfCode.Day08;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
[InputFile("test2.txt", InputFileType.Test)]
public abstract class Day08 : ISolution
{
    public void Run(string inputFile)
    { 
        var input = inputFile
            .AsSpan()
            .TrimEnd();

        // Map the input to be indexed as a grid
        input.GetGridMetadata(out var gridSize, out var rowSkip);

        RunDay8(input, gridSize, rowSkip);
    }

    protected abstract void RunDay8(ReadOnlySpan<char> input, int gridSize, int rowSkip);

    // private int GetGridSize(ReadOnlySpan<char> input)
    // {
    //     // Grid size == index of the first newline from the start of the input
    //     var gridSize = input.IndexOfAny('\r', '\n');
    //     if (gridSize < 1) throw new ArgumentException("First line does not have any content", nameof(input));
    //
    //     return gridSize;
    // }
    //
    // private static void GetInputMetadata(ReadOnlySpan<char> input, out int gridSize, out int newlineSkip)
    // {
    //     // Grid size == index of the first newline from the start of the input
    //     gridSize = input.IndexOfAny('\r', '\n');
    //     if (gridSize < 1) throw new ArgumentException("First line does not have any content", nameof(input));
    //     
    //     // Newline size == distance between the first newline character and the next non-newline character
    //     if (input[gridSize + 1] is not '\r' or '\n') newlineSkip = 1;
    //     else if (input[gridSize + 2] is not '\r' or '\n') newlineSkip = 2;
    //     else throw new ArgumentException("Newline is not \\r\\n (CRLF) or \\n (LF)", nameof(input));
    // }
    
    // Failed attempt at an optimized version
    // public void Run(string inputFile)
    // {
    //     var input = inputFile
    //         .AsSpan()
    //         .TrimEnd();
    //
    //     // Scan first line of input to compute metadata
    //     GetInputMetadata(input, out var gridSize, out var rowSkip);
    //
    //     // How far you can see by looking in from a particular edge.
    //     // Value is the index of the first occluded tree from a particular edge.
    //     // 0 = top
    //     // 1 = right
    //     // 2 = bottom
    //     // 3 = left
    //     var edges = new int[4, gridSize];
    //     
    //     // Populate grid
    //     for (var row = 0; row < gridSize; row++)
    //     {
    //         // Left
    //         var leftMax = '0'; // This feels so wrong
    //         for (var col = 0; col < gridSize; col++)
    //         {
    //             var tree = ReadGrid(row, col, rowSkip, input);
    //
    //             // Chec
    //             
    //             if (tree <= leftMax)
    //             {
    //                 edges
    //             }
    //
    //             leftMax = tree;
    //         }
    //         
    //         // Right
    //         for (var col = gridSize - 1; col >= 0; col--)
    //         {
    //             
    //         }
    //     }
    //     for (var col = 0; col < gridSize; col++)
    //     {
    //         // Top
    //         for (var row = 0; row < gridSize; row++)
    //         {
    //             
    //         }
    //         
    //         // Bottom
    //         for (var row = gridSize - 1; row >= 0; row--)
    //         {
    //             
    //         }
    //     }
    //     
    //     // Compute result
    // }
    //
    // private static char ReadGrid(int row, int col, int rowSkip, ReadOnlySpan<char> input)
    // {
    //     var rowStart = row * rowSkip;
    //     return input[rowStart + col];
    // }
    //
    // private static void GetInputMetadata(ReadOnlySpan<char> input, out int gridSize, out int rowSkip)
    // {
    //     // Grid size == index of the first newline from the start of the input
    //     gridSize = input.IndexOfAny('\r', '\n');
    //     if (gridSize < 1) throw new ArgumentException("First line does not have any content", nameof(input));
    //     
    //     // Newline size == distance between the first newline character and the next non-newline character
    //     if (input[gridSize + 1] is not '\r' or '\n') rowSkip = gridSize + 1;
    //     else if (input[gridSize + 2] is not '\r' or '\n') rowSkip = gridSize + 2;
    //     else throw new ArgumentException("Newline is not \\r\\n (CRLF) or \\n (LF)", nameof(input));
    // }
    protected static char ReadTree(int row, int col, ReadOnlySpan<char> input, int rowSkip)
    {
        var index = (row * rowSkip) + col;
        return input[index];
    }
}