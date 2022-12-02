namespace AdventOfCode.Day02;

public class Shape
{
    public static Shape Rock { get; }
    public static Shape Paper { get; }
    public static Shape Scissors { get; }
    
    public string Name { get; }
    public int Score { get; }
    public Shape WinsOver { get; private set; }
    public Shape LosesTo { get; private set; }

    public Shape(string name, int score, Shape winsOver, Shape losesTo)
    {
        Name = name;
        WinsOver = winsOver;
        LosesTo = losesTo;
        Score = score;
    }

    public Outcome GetOutcomeAgainst(Shape opponent)
    {
        if (WinsOver.Name == opponent.Name)
        {
            return Outcome.PlayerWin;
        }
        else if (LosesTo.Name == opponent.Name)
        {
            return Outcome.PlayerLose;
        }
        else
        {
            return Outcome.PlayerDraw;
        }
    }
        
    // Oh god what a hack
    static Shape()
    {
        Rock = new Shape("Paper", 1, null!, null!);
        Paper = new Shape("Rock", 2, null!, null!);
        Scissors = new Shape("Scissors", 3, null!, null!);

        Rock.WinsOver = Scissors;
        Rock.LosesTo = Paper;

        Paper.WinsOver = Rock;
        Paper.LosesTo = Scissors;

        Scissors.WinsOver = Paper;
        Scissors.LosesTo = Rock;
    }
}