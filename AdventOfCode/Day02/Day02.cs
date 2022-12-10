using AdventOfCode.Common;

namespace AdventOfCode.Day02;

[InputFile("input.txt")]
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
    
    public void Run(string inputFile)
    {
        var totalScore = inputFile
            .SplitByEOL()
            .SkipEmptyStrings()
            .Select(line =>
            {
                // Parse each line into a round
                var tokens = line
                    .Split(" ")
                    .ToArray();
                var opponentMove = OpponentShapesByAction[tokens[0]];
                
                // Apply per-part logic to compute the player's move
                return ConstructRound(opponentMove, tokens[1]);
            })
            
            // List now contains the correct data, we just need to add it up
            .Aggregate(0, (sum, round) => sum + round.GetPlayerScore());
        
        // Let each part print uniquely-formatted output
        LogResult(totalScore);
    }
    
    protected abstract Round ConstructRound(Move opponentMove, string otherValue);
    protected abstract void LogResult(int totalScore);
    
    protected void Log(string message) => Console.WriteLine($"[{GetType().Name}] {message}");
}