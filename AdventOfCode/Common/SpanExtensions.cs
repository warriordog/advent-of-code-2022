namespace AdventOfCode.Common;

public static class SpanExtensions
{
    [Obsolete("This is non-idiomatic - please use Span<T>.Slice().IndexOf() instead")]
    public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex = 0)
    {
        if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex), "startIndex must be at least zero");
        if (startIndex >= span.Length) throw new ArgumentOutOfRangeException(nameof(startIndex), "startIndex must be less than span length");

        for (var i = startIndex; i < span.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(value, span[i]))
            {
                return i;
            } 
        }

        return -1;
    }
    
    public static void GetGridMetadata(this ReadOnlySpan<char> input, out int rowSize, out int rowSkip)
    {
        // Row size == index of the first newline from the start of the input
        rowSize = input.IndexOfAny('\r', '\n');
        if (rowSize < 1) throw new ArgumentException("First line does not have any content", nameof(input));
        
        // Newline size == distance between the first newline character and the next non-newline character
        if (input[rowSize + 1] is not '\r' or '\n') rowSkip = rowSize + 1;
        else if (input[rowSize + 2] is not '\r' or '\n') rowSkip = rowSize + 2;
        else throw new ArgumentException("Newline is not \\r\\n (CRLF) or \\n (LF)", nameof(input));
    }

    /// <summary>
    /// Split a span into lines.
    /// Lines are delimited by either "\n" or "\r\n".
    /// </summary>
    /// <param name="span">Span to split</param>
    /// <returns>Returns an iterator that will produce all lines in the span</returns>
    public static SpanLineSplitter SplitLines(this ReadOnlySpan<char> span) => new(span);
}

public ref struct SpanLineSplitter
{
    public ReadOnlySpan<char> Current { get; private set; }
    private ReadOnlySpan<char> _remaining;
    
    public SpanLineSplitter(ReadOnlySpan<char> span)
    {
        _remaining = span;
        Current = default;
    }

    public bool MoveNext()
    {
        if (_remaining.Length < 1)
            return false;
        
        int newlineStart; // This points to the first character of the newline token
        int newlineEnd; // This points the the first character *after* the newline token
        
        // This is a separate span because we need to slice multiple times while scanning, but we want the entire input available to return.
        // This copy can be mutated as-needed so that _remaining can remain pristine.
        var remainingSearch = _remaining;
         
        // Find the next newline.
        // This can involve multiple loops if the input contains lone \r tokens.
        while (true)
        {
            // Jump to next candidate
            newlineStart = remainingSearch.IndexOfAny('\r', '\n');
            
            // If we don't find a match, then this is the end of the line.
            // Set newlineStart and newlineEnd to be just past the last character
            if (newlineStart < 0)
            {
                newlineStart = _remaining.Length;
                newlineEnd = _remaining.Length;
                break;
            }
         
            // Check for \n, which is valid by itself
            if (remainingSearch[newlineStart] == '\n')
            {
                newlineEnd = newlineStart + 1;
                break;
            }
            
            // If we get here then the newline starts with \r.
            // Special handling is needed.
            
            // If there are no more characters, then this is a lone \r which is part of the input.
            // Push newlineStart and newlineEnd past the end-of-line and break for downstream handling.
            if (newlineStart == remainingSearch.Length - 1)
            {
                newlineStart++;
                newlineEnd = newlineStart;
                break;
            }

            // If the next character is \n, then this is \r\n.
            // We need to consume two characters for the newline.
            if (remainingSearch[newlineStart + 1] == '\n')
            {
                newlineEnd = newlineStart + 2;
                break;
            }
            
            // Otherwise, this is a lone \r and a false-positive.
            // We need to move past it and repeat the search.
            remainingSearch = remainingSearch[(newlineStart + 1)..];
        }
        
        // We found the next next newline!
        // The next token always starts at index 0 so we can just slice up to the start of the newline.
        // newlineStart can be equal to (but not greater than) the the length of _remaining, but this is an exclusive slice that's OK.
        Current = _remaining[..newlineStart];

        // Update _remaining to start *after* the newline.
        // The "newline" might have actually been the end-of-input so we need to check the bounds.
        if (newlineEnd < _remaining.Length)
            _remaining = _remaining[newlineEnd..];
        else
            _remaining = ReadOnlySpan<char>.Empty;
        
        return true;
    }

    /// <summary>
    /// Moves to the next token in the input, then returns it for convenience.
    /// If there are no more tokens, then returns an empty span.
    /// </summary>
    /// <returns>Returns the next token in the input</returns>
    public ReadOnlySpan<char> MoveNextAndGet()
    {
        // Check for end of input
        if (!MoveNext())
        {
            // Maybe this should be an exception?
            // Or use TryGet pattern?
            return default;
        }

        return Current;
    }
    
    public SpanLineSplitter GetEnumerator() => this;
}

// TODO implement Split(ReadOnlySpan<char> input, ReadOnlySpan<char> delemiter)