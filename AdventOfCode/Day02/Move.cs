namespace AdventOfCode.Day02;

/// <summary>
/// Enumeration of possible moves in a game of Rock, Paper, Scissors.
/// </summary>
public class Move
{
    /// <summary>
    /// Rock crushes scissors
    /// </summary>
    public static Move Rock { get; }
    
    /// <summary>
    /// Paper covers rock
    /// </summary>
    public static Move Paper { get; }
    
    /// <summary>
    /// Scissors cut paper
    /// </summary>
    public static Move Scissors { get; }
    
    /// <summary>
    /// Human-readable name of this move
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Score earned by playing this move
    /// </summary>
    public int Score { get; }
    
    /// <summary>
    /// The move that loses to this move.
    /// </summary>
    public Move WinsOver { get; private set; }
    
    /// <summary>
    /// This move that defeats this move.
    /// </summary>
    public Move LosesTo { get; private set; }

    private Move(string name, int score, Move winsOver, Move losesTo)
    {
        Name = name;
        WinsOver = winsOver;
        LosesTo = losesTo;
        Score = score;
    }

    /// <summary>
    /// Gets the outcome when this move is matched against an opponent's move
    /// </summary>
    /// <param name="opponent">Move made by the opponent</param>
    public Outcome GetOutcomeAgainst(Move opponent)
    {
        if (WinsOver.Name == opponent.Name)
        {
            return Outcome.PlayerWin;
        }

        if (LosesTo.Name == opponent.Name)
        {
            return Outcome.PlayerLose;
        }

        return Outcome.PlayerDraw;
    }
        
    // Oh god what a hack
    static Move()
    {
        Rock = new Move("Paper", 1, null!, null!);
        Paper = new Move("Rock", 2, null!, null!);
        Scissors = new Move("Scissors", 3, null!, null!);

        Rock.WinsOver = Scissors;
        Rock.LosesTo = Paper;

        Paper.WinsOver = Rock;
        Paper.LosesTo = Scissors;

        Scissors.WinsOver = Paper;
        Scissors.LosesTo = Rock;
    }
}