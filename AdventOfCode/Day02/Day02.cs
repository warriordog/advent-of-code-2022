namespace AdventOfCode.Day02;

public abstract class Day02 : ISolution
{
    /// <summary>
    /// Maps input tokens to opponent actions.
    /// </summary>
    /// <remarks>
    /// The first column is what your opponent is going to play: A for Rock, B for Paper, and C for Scissors.
    /// </remarks>
    private static Dictionary<string, Move> OpponentShapesByAction { get; } = new()
    {
        { "A", Move.Rock },
        { "B", Move.Paper },
        { "C", Move.Scissors }
    };
    
    public async Task Run(string inputFile, List<string> solutionArgs)
    {
        var inputLines = await File.ReadAllLinesAsync(inputFile);

        var input = inputLines
            .Where(line => line.Length > 0)
            .Select(line =>
            {
                var tokens = line
                    .Split(" ")
                    .ToArray();
                var opponentMove = OpponentShapesByAction[tokens[0]];
                return (opponentMove, tokens[1]);
            });
        
        RunDay2(input);
    }

    protected abstract void RunDay2(IEnumerable<(Move OpponentMove, string OtherValue)> input);
    protected void Log(string message) => Console.WriteLine($"[{GetType().Name}] {message}");
    
    /// <summary>
    /// Compute the total score for a collection of rounds
    /// </summary>
    /// <remarks>
    /// The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors) plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).
    /// </remarks>
    protected static int ComputeTotalScore(IEnumerable<Round> rounds) => rounds.Aggregate(0, (sum, round) => sum + round.GetPlayerScore());
}