using System.Text.Json;
using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day13;

[Solution("Day13", "Part1")]
public class Day13Part1 : Day13
{
    private readonly ILogger<Day13Part1> _logger;
    public Day13Part1(ILogger<Day13Part1> logger) => _logger = logger;
    
    protected override void RunDay13(SpanLineSplitter inputLines)
    {
        var inOrderSum = 0; // Sum up the indices of all pairs that are in order
        var pairIndex = 1; // Starts at 1, as usual for AoC
        do
        {
            // Read next pair as JSON
            var firstJson = inputLines.MoveNextAndGet();
            var secondJson = inputLines.MoveNextAndGet();
            var first = JsonSerializer.Deserialize(firstJson, PacketValueContext.Default.PacketValue)
                        ?? throw new ArgumentException($"Input is invalid - line did not deserialize correctly: {firstJson}", nameof(inputLines));
            var second = JsonSerializer.Deserialize(secondJson, PacketValueContext.Default.PacketValue)
                         ?? throw new ArgumentException($"Input is invalid - line did not deserialize correctly: {secondJson}", nameof(inputLines));
            
            // Check if its in the right order
            if (first.CompareTo(second) < 0)
            {
                inOrderSum += pairIndex;
            }
            
            // Move to next
            pairIndex++;
        } while (inputLines.MoveNext()); // Skip the next line *and* check for additional input, all in one action
        
        _logger.LogInformation("The sum of the indices of all in-order pairs is [{inOrderSum}].", inOrderSum);
    }
}