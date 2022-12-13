using System.Text.Json;
using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day13;

[Solution("Day13", "Part2")]
public class Day13Part2 : Day13
{
    private readonly ILogger<Day13Part2> _logger;
    public Day13Part2(ILogger<Day13Part2> logger) => _logger = logger;

    protected override void RunDay13(SpanLineSplitter inputLines)
    {
        // Start with the two divider packets
        var divider1 = new PacketValue(new[] { new PacketValue(new[] { new PacketValue(2) }) }); // [[2]]
        var divider2 = new PacketValue(new[] { new PacketValue(new[] { new PacketValue(6) }) }); // [[6]]
        var packets = new List<PacketValue> { divider1, divider2 };

        
        // Add each packet from input
        foreach (var line in inputLines)
        {
            // Skip empty lines
            if (line.Length == 0)
                continue;

            // Parse the packet as JSON
            var packet = JsonSerializer.Deserialize(line, PacketValueContext.Default.PacketValue)
                         ?? throw new ArgumentException($"Input is invalid - line did not deserialize correctly: {line}", nameof(inputLines));
            
            packets.Add(packet);
        }
        
        // Sort packets
        packets.Sort();
        
        // Find divider packets (1-indexed)
        var divider1Index = packets.IndexOf(divider1) + 1;
        var divider2Index = packets.IndexOf(divider2) + 1;

        // Compute decoder key.
        // Who designed this algorithm??
        // The elves should fire their engineer.
        var decoderKey = divider1Index * divider2Index;
        
        _logger.LogInformation("The decoder key is [{decoderKey}].", decoderKey);
    }
}