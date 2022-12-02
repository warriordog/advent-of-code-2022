namespace AdventOfCode.Day02;

public class Day02Part1 : Day02
{
    private static Dictionary<string, Shape> PlayerShapesByAction { get; } = new()
    {
        { "X", Shape.Rock },
        { "Y", Shape.Paper },
        { "Z", Shape.Scissors }
    };
    
    protected override void RunDay2(IEnumerable<(Shape OpponentMove, string OtherValue)> input)
    {
        var rounds = input
            .Select(i =>
            {
                var player = PlayerShapesByAction[i.OtherValue];
                return new Round(player, i.OpponentMove);
            })
            .ToList();
        
        var totalScore = ComputeTotalScore(rounds);
        Console.WriteLine($"[Day02 Part1] The total score from the strategy guide is [{totalScore}].");
    }
}