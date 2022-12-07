using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day02;

[Solution("Day02", "Part2")]
[InputFile("input.txt", resolution: InputFileResolution.PathRelativeToSolution)]
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
    
    private readonly ILogger<Day02Part2> _logger;
    public Day02Part2(ILogger<Day02Part2> logger) => _logger = logger;
    
    protected override Round ConstructRound(Move opponentMove, string otherValue)
    {
        var desiredOutcome = OutcomesByAction[otherValue];
        var playerMove = FindPlayerMoveToResultInOutcome(opponentMove, desiredOutcome);
        return new Round(playerMove, opponentMove);
    }

    private static Move FindPlayerMoveToResultInOutcome(Move opponentMove, Outcome desiredOutcome)
    {
        if (desiredOutcome == Outcome.PlayerWin) return opponentMove.LosesTo;
        if (desiredOutcome == Outcome.PlayerLose) return opponentMove.WinsOver;
        return opponentMove;
    }
    
    protected override void LogResult(int totalScore) => _logger.LogInformation($"If everything goes according to the guide, then the total score will be [{totalScore}].");
}