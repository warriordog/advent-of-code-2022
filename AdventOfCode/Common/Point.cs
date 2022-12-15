namespace AdventOfCode.Common;

/// <summary>
/// Represents a point in 2D cartesian space.
/// Origin (0,0) is the top-left corner.
/// Y axis (row) extends down, and X axis (column) extends right.
/// </summary>
/// <param name="Row">Y position, starting at zero and increasing downward from the top-left corner.</param>
/// <param name="Col">X position, starting at zero and increasing right from the top-left corner.</param>
public readonly record struct Point(long Row, long Col)
{
    /// <summary>
    /// Returns a new point that is equal to this point moved by the specified number of positions in either axis.
    /// </summary>
    /// <param name="rowOffset"></param>
    /// <param name="colOffset"></param>
    /// <returns></returns>
    public Point MoveBy(long rowOffset, long colOffset) => new(Row + rowOffset, Col + colOffset);
    
    public Point GetNeighbor(Direction direction) => direction switch
    {
        Direction.Up => this with { Row = Row - 1 },
        Direction.Down => this with { Row = Row + 1 },
        Direction.Right => this with { Col = Col + 1 },
        Direction.Left => this with { Col = Col - 1 },
        Direction.None => this,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), "Direction must be a valid value of Dir enum.")
    };

    public override string ToString() => $"({Row}, {Col})";
}

public enum Direction
{
    None,
    Up,
    Down,
    Right,
    Left
}