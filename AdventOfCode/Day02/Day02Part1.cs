using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day02;

[Solution("Day02", "Part1")]
[InputFile("input.txt")]
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
    
    private readonly ILogger<Day02Part1> _logger;
    public Day02Part1(ILogger<Day02Part1> logger) => _logger = logger;
    

    protected override Round ConstructRound(Move opponentMove, string otherValue)
    {
        var player = PlayerShapesByAction[otherValue];
        return new Round(player, opponentMove);
    }
    
    protected override void LogResult(int totalScore) => _logger.LogInformation($"The total score from the strategy guide is [{totalScore}].");
}