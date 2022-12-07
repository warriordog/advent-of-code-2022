using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day06;

[Solution("Day06", "Part2", "BitFields")]
public class Day06Part2BitFields : ISolution
{
    private readonly ILogger<Day06Part2BitFields> _logger;
    public Day06Part2BitFields(ILogger<Day06Part2BitFields> logger) => _logger = logger;

    public void Run(string inputFile)
    {
        // Convert input into bytes
        var dataEnd = 0;
        var data = new byte[inputFile.Length];
        for (var i = 0; i < inputFile.Length; i++)
        {
            var input = inputFile[i];
            if (input is '\r' or '\n') continue;

            data[i] = (byte)inputFile[i];
            dataEnd = i;
        }

        var dataStream = data.AsSpan(0, dataEnd);


        var headerStart1 = FindUniqueSequenceBitFields(dataStream, 14);
        var headerStart = headerStart1;
        var toSkip = headerStart + 14;
        _logger.LogInformation("The communicator must process [{toSkip}] characters before the first 14-character start-of-message marker is complete.", toSkip);   
    }


    private static int FindUniqueSequenceBitFields(ReadOnlySpan<byte> stream, int sequenceLength)
    {
        if (sequenceLength < 1) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be at least 1");
        if (sequenceLength >= stream.Length) throw new ArgumentOutOfRangeException(nameof(sequenceLength), "sequenceLength must be less than the length of stream");
        
        
        // A great big hack.
        // Each byte in the array is a bitfield representing if the character has been seen.
        // The state of a character can be checked like this:
        //  isSet = (seenChars[chr / 8] >> (chr % 8)) & 0x01;
        // And mark as found like this:
        //  seenChars[chr / 8] |= 0x01 << (chr % 8);
        // And clear mark like this:
        //  seenChars[chr / 8] &= ~(0x01 << (chr % 8));
        var seenChars = new byte[256 / 8];
        bool IsSet(byte idx) => ((seenChars[idx / 8] >> (idx % 8)) & 0x01) == 0x01;
        void SetFlag(byte idx) => seenChars[idx / 8] |= (byte)(0x01 << (idx % 8));
        void ClearFlag(byte idx) => seenChars[idx / 8] &= (byte)~(0x01 << (idx % 8));
        
        // Another hack - avoid nested loops by working with a window
        var windowStart = 0;
        var windowEnd = 0;

        // Pre-add the first character, otherwise it will get skipped
        SetFlag(stream[windowStart]);
        
        // Move the window forward in an "inchworm" pattern.
        // We have to add 1 here because windowStart/windowEnd and INCLUSIVE!
        // The difference is NOT equal to the number of characters!
        while ((windowEnd - windowStart) + 1 < sequenceLength)
        {
            // Extend the window
            windowEnd++;
            
            // Add the next character
            var nextChar = stream[windowEnd];
            
            // If we ran into a duplicate, then roll forward until the first copy drops off
            while (IsSet(nextChar))
            {
                ClearFlag(stream[windowStart]);
                windowStart++;
            }
            
            // Mark next character as seen.
            // This has to be last, because the flag will be cleared when we remove the duplicates in the previous loop
            SetFlag(nextChar);
        }

        return windowStart;
    }
}