namespace AdventOfCode.Day02;

public class Day02Part2 : Day02
{
    private static Dictionary<string, Outcome> OutcomesByAction { get; } = new()
    {
        { "X", Outcome.PlayerLose },
        { "Y", Outcome.PlayerDraw },
        { "Z", Outcome.PlayerWin }
    };

    protected override void RunDay2(IEnumerable<(Shape OpponentMove, string OtherValue)> input)
    {
        var rounds = input
            .Select(i =>
            {
                var desiredOutcome = OutcomesByAction[i.OtherValue];
                var playerMove = FindPlayerMoveToResultInOutcome(i.OpponentMove, desiredOutcome);
                return new Round(playerMove, i.OpponentMove);
            })
            .ToList();
        
        
        var totalScore = ComputeTotalScore(rounds);
        Console.WriteLine($"[Day02 Part2] If everything goes according to the guide, then the total score will be [{totalScore}].");
    }

    private Shape FindPlayerMoveToResultInOutcome(Shape opponentMove, Outcome desiredOutcome)
    {
        if (desiredOutcome == Outcome.PlayerWin) return opponentMove.LosesTo;
        if (desiredOutcome == Outcome.PlayerLose) return opponentMove.WinsOver;
        return opponentMove;
    }
}