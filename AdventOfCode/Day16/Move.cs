namespace AdventOfCode.Day16;

// Because writing "List<List<Move>>" gets REALLY old, REALLY fast.
public class MoveSet : List<Move> {}

public readonly record struct Move(int ValveIndex, bool OpenValve);