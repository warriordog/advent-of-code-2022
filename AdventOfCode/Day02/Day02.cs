namespace AdventOfCode.Day02;

public abstract class Day02 : ISolution
{
    private static Dictionary<string, Shape> OpponentShapesByAction { get; } = new()
    {
        { "A", Shape.Rock },
        { "B", Shape.Paper },
        { "C", Shape.Scissors }
    };
    
    public async Task Run(string inputFile, List<string> solutionArgs)
    {
        var inputLines = await File.ReadAllLinesAsync(inputFile);

        var input = inputLines
            .Where(line => line.Length > 0)
            .Select(line =>
            {
                var moves = line
                    .Split(" ")
                    .ToArray();
                var opponentMove = OpponentShapesByAction[moves[0]];
                return (opponentMove, moves[1]);
            });
        
        RunDay2(input);
    }

    protected abstract void RunDay2(IEnumerable<(Shape OpponentMove, string OtherValue)> input);
    protected static int ComputeTotalScore(List<Round> moves)
    {
        var totalScore = moves.Aggregate(0, (sum, round) => sum + round.GetPlayerScore());
        return totalScore;
    }
}