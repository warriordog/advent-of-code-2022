using System.Numerics;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day06;

[Solution("Day06", "Part2", "XOR")]
public class Day06Part2XOR : ISolution
{
    private readonly ILogger<Day06Part2XOR> _logger;
    public Day06Part2XOR(ILogger<Day06Part2XOR> logger) => _logger = logger;

    public void Run(string inputFile)
    {
        var dataStream = inputFile
            .AsSpan()
            .TrimEnd();
        var toSkip = SkipUniqueSequenceXOR(dataStream, 14);
        _logger.LogInformation("The communicator must process [{toSkip}] characters before the first 14-character start-of-message marker is complete.", toSkip);   
    }

    // Performance trick by /u/mkeeter.
    // This only works for characters in range a-z.
    private static int SkipUniqueSequenceXOR(ReadOnlySpan<char> stream, int sequenceLength)
    {
        if (sequenceLength < 1) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be at least 1");
        if (sequenceLength >= stream.Length) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be less than the length of stream");
        if (sequenceLength > 32) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "This implementation supports a max sequenceLength of 32");
        
        // Bitmasked flags for each character.
        // If a 1 is set, then that character has been seen an odd number of times.
        // For given window size, all bits can only be 1 if each character appears exactly once.
        var charFlags = 0x0000u;

        // Process each character
        for (var i = 0; i < stream.Length; i++)
        {
            // Convert the next character into a bitmask
            var charMask = 0x0001u << (byte)(stream[i] - 'a');
            
            // Update its flag
            charFlags ^= charMask;
            
            // Handle the tail end once the window is large enough
            if (i >= sequenceLength)
            {
                // Pop the last character
                charFlags ^= 0x0001u << (byte)(stream[i - sequenceLength] - 'a');
            
                // Check for success.
                if (BitOperations.PopCount(charFlags) == sequenceLength)
                {
                    return i + 1;
                }
            }
        }

        throw new ApplicationException("Algorithm failure - did not find a match in the input");
    }
}