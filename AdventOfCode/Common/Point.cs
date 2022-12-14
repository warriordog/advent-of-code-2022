namespace AdventOfCode.Common;

/// <summary>
/// Represents a point in 2D cartesian space.
/// Origin (0,0) is the top-left corner.
/// Y axis (row) extends down, and X axis (column) extends right.
/// </summary>
/// <param name="Row">Y position, starting at zero and increasing downward from the top-left corner.</param>
/// <param name="Col">X position, starting at zero and increasing right from the top-left corner.</param>
public readonly record struct Point(int Row, int Col)
{
    /// <summary>
    /// Returns a new point that is equal to this point moved by the specified number of positions in either axis.
    /// </summary>
    /// <param name="rowOffset"></param>
    /// <param name="colOffset"></param>
    /// <returns></returns>
    public Point MoveBy(int rowOffset, int colOffset) => new(Row + rowOffset, Col + colOffset);
}