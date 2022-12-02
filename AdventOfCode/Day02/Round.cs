namespace AdventOfCode.Day02;

/// <summary>
/// A single round of Rock, Paper, Scissors.
/// </summary>
public class Round
{
    /// <summary>
    /// The player's move
    /// </summary>
    public Move PlayerMove { get; }
    
    /// <summary>
    /// The opponent's move
    /// </summary>
    public Move OpponentMove { get; }
    
    public Round(Move playerMove, Move opponentMove)
    {
        PlayerMove = playerMove;
        OpponentMove = opponentMove;
    }

    /// <summary>
    /// Computes a player's score at the end of this round
    /// </summary>
    /// <remarks>
    /// The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors) plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).
    /// </remarks>
    public int GetPlayerScore() => PlayerMove.Score + PlayerMove.GetOutcomeAgainst(OpponentMove).ScoreBonus;
}