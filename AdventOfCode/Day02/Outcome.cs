namespace AdventOfCode.Day02;

/// <summary>
/// Enumeration of possible outcomes for a round.
/// </summary>
public class Outcome
{
    /// <summary>
    /// The player won
    /// </summary>
    public static Outcome PlayerWin { get; } = new("Win", 6);
    
    /// <summary>
    /// The round was a draw - nobody won
    /// </summary>
    public static Outcome PlayerDraw { get; } = new("Draw", 3);
    
    /// <summary>
    /// The player lost
    /// </summary>
    public static Outcome PlayerLose { get; } = new("Lose", 0);
    
    /// <summary>
    /// Human-readable name of the outcome
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Score bonus received by the player after this round.
    /// Can be zero.
    /// </summary>
    public int ScoreBonus { get; }

    private Outcome(string name, int scoreBonus)
    {
        Name = name;
        ScoreBonus = scoreBonus;
    }
}