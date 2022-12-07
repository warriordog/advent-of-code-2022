using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day06;

[Solution("Day06", "Part2", "SmallList")]
public class Day06Part2SmallList : ISolution
{
    private readonly ILogger<Day06Part2SmallList> _logger;
    public Day06Part2SmallList(ILogger<Day06Part2SmallList> logger) => _logger = logger;

    public void Run(string inputFile)
    {
        var dataStream = inputFile
            .AsSpan()
            .TrimEnd();
        var headerStart = FindUniqueSequenceSmallList(dataStream, 14);
        var toSkip = headerStart + 14;
        _logger.LogInformation("The communicator must process [{toSkip}] characters before the first 14-character start-of-message marker is complete.", toSkip);   
    }

    private static int FindUniqueSequenceSmallList(ReadOnlySpan<char> stream, int sequenceLength)
    {
        if (sequenceLength < 1) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be at least 1");
        if (sequenceLength >= stream.Length) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be less than the length of stream");
        
        // A little hack for performance:
        // Track the number of instances of each character and update as we scroll through.
        // Only 26 indexes because input only has 26 possible characters
        var seenChars = new byte[26];
        
        // Another hack - avoid nested loops by working with a window
        var windowStart = 0;
        var windowEnd = 0;

        // Pre-add the first character, otherwise it will get skipped
        seenChars[stream[windowStart] - 'a'] = 1;
        
        // Move the window forward in an "inchworm" pattern.
        // We have to add 1 here because windowStart/windowEnd and INCLUSIVE!
        // The difference is NOT equal to the number of characters!
        while ((windowEnd - windowStart) + 1 < sequenceLength)
        {
            // Extend the window
            windowEnd++;
            
            // Add the next character
            var nextChar = stream[windowEnd] - 'a';
            seenChars[nextChar]++;
            
            // If we ran into a duplicate, then roll forward until the first copy drops off
            while (seenChars[nextChar] > 1)
            {
                var lastChar = stream[windowStart] - 'a';
                seenChars[lastChar]--;
                windowStart++;
            }
        }

        return windowStart;
    }
}