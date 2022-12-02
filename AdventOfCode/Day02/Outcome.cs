namespace AdventOfCode.Day02;

public class Outcome
{
    public static Outcome PlayerWin { get; } = new("Win", 6);
    public static Outcome PlayerDraw { get; } = new("Draw", 3);
    public static Outcome PlayerLose { get; } = new("Lose", 0);
    
    public string Name { get; }
    public int ScoreBonus { get; }

    private Outcome(string name, int scoreBonus)
    {
        Name = name;
        ScoreBonus = scoreBonus;
    }
}