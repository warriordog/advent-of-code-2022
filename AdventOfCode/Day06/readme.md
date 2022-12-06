# Day 6: Tuning Trouble

I had the misfortune of implementing part 1 in the ONLY way that doesn't also work for part 2.
I assumed (incorrectly) that the input would be some massive string with 10s of thousands of characters, so I decided to implement a "naive optimized" solution that was algorithmically inefficient but somewhat micro-optimized.
It worked, but could only search for tokens of exactly 4 unique characters.

So I had to rewrite everything for part 2, but my new solution is way better.
It can find non-repeating sequences of any length, in an input of any length, with any ASCII characters (not just a-z).
Despite the flexibility, it completes part 2 in ~21.5 μs and part 1 in only **5.7 μs**!
That's nothing impressive for C or Rust, but for managed C#, that's quite good!

The algorithm that I wrote makes only a single pass over the input.
It uses a sliding window that grows and shrinks like an inchworm to avoid ever needing to backtrack.
Duplication is checked by tracking an array of 256 bytes, where the index is the character code of a token and the value is how many times that character appears within the window.
By only moving by one character at a time, its possible to guarantee accuracy of that table without ever backtracking.
The basic process looks like this:
1. Starting with the window at [0,0], begin looping until the window has reached the target size.
2. On each iteration, extend the end index by 1 and read the next character. Increment its count in the table.
3. Check if the count is greater than 1.
4. If so, then "inch forward" by dropping off the first character in the window. This means decrementing it in the table and increasing the window's start index.
5. Repeat from step 3 until the count is less than or equal to 1.
6. When the outer loop completes, windowStart will be the start of the marker. Add the marker length to get the result.

I'm sure there are better ways, but I'm quite proud of this one!