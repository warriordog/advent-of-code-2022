namespace AdventOfCode.Day16;

public class MoveSet : List<Move> {}

public readonly record struct Move(int ValveIndex, bool OpenValve);