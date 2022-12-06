using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day06;

public abstract class Day06 : ISolution
{
    protected readonly ILogger Logger;
    protected Day06(ILogger logger) => Logger = logger;
    
    public void Run(string inputFile)
    {
        var dataStream = inputFile
            .AsSpan()
            .TrimEnd();

        RunDay6(dataStream);
    }
    
    protected abstract void RunDay6(ReadOnlySpan<char> dataStream);

    protected static int FindUniqueSequence(ReadOnlySpan<char> stream, int sequenceLength)
    {
        if (sequenceLength < 1) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be at least 1");
        if (sequenceLength >= stream.Length) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be less than the length of stream");
        
        // A little hack for performance:
        // Track the number of instances of each character and update as we scroll through.
        // Takes a lot of memory and probably ruins the cache, but its still way faster than brute force.
        var seenChars = new byte[256];
        
        // Another hack - avoid nested loops by working with a window
        var windowStart = 0;
        var windowEnd = 0;

        // Pre-add the first character, otherwise it will get skipped
        seenChars[stream[windowStart]] = 1;
        
        // Move the window forward in an "inchworm" pattern.
        // We have to add 1 here because windowStart/windowEnd and INCLUSIVE!
        // The difference is NOT equal to the number of characters!
        while ((windowEnd - windowStart) + 1 < sequenceLength)
        {
            // Extend the window
            windowEnd++;
            if (windowEnd >= stream.Length) throw new ApplicationException("Algorithm failure - windowEnd has exceeded the dataStream bounds");
            
            // Add the next character
            var nextChar = stream[windowEnd];
            seenChars[nextChar]++;
            
            // If we ran into a duplicate, then roll forward until the first copy drops off
            while (seenChars[nextChar] > 1)
            {
                // Drop the first character and shrink the window
                var lastChar = stream[windowStart];
                seenChars[lastChar]--;
                windowStart++;

                // Sanity check - can be removed for performance
                if (windowStart > windowEnd) throw new ApplicationException("Algorithm failure - windowStart has exceeded windowEnd");
            }
        }

        return windowStart;
    }
}