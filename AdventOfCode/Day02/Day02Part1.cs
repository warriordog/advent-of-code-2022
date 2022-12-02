namespace AdventOfCode.Day02;

public class Day02Part1 : Day02
{
    /// <summary>
    /// Maps input tokens to player movements
    /// </summary>
    /// <remarks>
    /// The second column, you reason, must be what you should play in response: X for Rock, Y for Paper, and Z for Scissors.
    /// </remarks>
    private static Dictionary<string, Move> PlayerShapesByAction { get; } = new()
    {
        { "X", Move.Rock },
        { "Y", Move.Paper },
        { "Z", Move.Scissors }
    };
    
    protected override void RunDay2(IEnumerable<(Move OpponentMove, string OtherValue)> input)
    {
        var rounds = input
            .Select(i =>
            {
                var player = PlayerShapesByAction[i.OtherValue];
                return new Round(player, i.OpponentMove);
            });
        
        var totalScore = ComputeTotalScore(rounds);
        Console.WriteLine($"[Day02 Part1] The total score from the strategy guide is [{totalScore}].");
    }
}