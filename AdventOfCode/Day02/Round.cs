namespace AdventOfCode.Day02;

public class Round
{
    public Shape PlayerMove { get; }
    public Shape OpponentMove { get; }
    
    public Round(Shape playerMove, Shape opponentMove)
    {
        PlayerMove = playerMove;
        OpponentMove = opponentMove;
    }

    public int GetPlayerScore()
    {
        return PlayerMove.Score + PlayerMove.GetOutcomeAgainst(OpponentMove).ScoreBonus;
    }
}