namespace AdventOfCode.Day02;

public class Day02Part2 : Day02
{
    /// <summary>
    /// Map input tokens to desired outcomes
    /// </summary>
    /// <remarks>
    /// The Elf finishes helping with the tent and sneaks back over to you.
    /// "Anyway, the second column says how the round needs to end: X means you need to lose, Y means you need to end the round in a draw, and Z means you need to win.
    /// Good luck!"
    /// </remarks>
    private static Dictionary<string, Outcome> OutcomesByAction { get; } = new()
    {
        { "X", Outcome.PlayerLose },
        { "Y", Outcome.PlayerDraw },
        { "Z", Outcome.PlayerWin }
    };

    protected override void RunDay2(IEnumerable<(Move OpponentMove, string OtherValue)> input)
    {
        var rounds = input
            .Select(i =>
            {
                var desiredOutcome = OutcomesByAction[i.OtherValue];
                var playerMove = FindPlayerMoveToResultInOutcome(i.OpponentMove, desiredOutcome);
                return new Round(playerMove, i.OpponentMove);
            });
        
        
        var totalScore = ComputeTotalScore(rounds);
        Console.WriteLine($"[Day02 Part2] If everything goes according to the guide, then the total score will be [{totalScore}].");
    }

    
    private static Move FindPlayerMoveToResultInOutcome(Move opponentMove, Outcome desiredOutcome)
    {
        if (desiredOutcome == Outcome.PlayerWin) return opponentMove.LosesTo;
        if (desiredOutcome == Outcome.PlayerLose) return opponentMove.WinsOver;
        return opponentMove;
    }
}