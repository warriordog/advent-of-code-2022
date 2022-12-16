# Day 15: Beacon Exclusion Zone

I was a day late on this one, which was partially due to the difficulty and partially due to general busyness in life.
My part 1 solution is quite efficient, running in about 30us without minimal optimization.
Part 2, on the other hand, is half-brute-force and takes about 7 seconds to complete.

Both parts were implemented with the help of three custom data structures `struct Point`, `struct HLine`, and `class CompoundHLine`.
`Point` is the same point structure that I've used several times this year.
I factored it out into a "Common" namespace so that it can be reused.
`HLine` is unique to day 15.
It stores 64-bit start and end points and computes the manhattan distance and total area (end-inclusive manhattan distance).

`CompoundHLine` is where the fun stuff happens.
This class stores a list of "segments" which are just individual HLines.
Whenever a new HLine is added to a CompoundHLine, the new line is chopped, resized, and merged with the existing segments.
This creates a sparse line where the total used area can be computed by adding the area of each component segments.
For part 1, that is 99% of the answer.
The last step is just to subtract any beacons that overlap the row.

For part 2, there's an extra step.
I couldn't find an efficient way to search in both axes, but I had a highly efficient way to search just one.
4000000 isn't *that* big of a number, so I just brute-force search the Y axis and generate a CompoundHLine for each row.
If the area of that line is equal to 4000000, then the missing beacon must be on that row.
Finding it is simple, as there are only three possible configurations for the HLine segments on that row:
```
0.......E   // E == 20 for test, 4000000 for prod inputs
[------].   // 1 segment, gap (missing beacon) is at the end. Get only segment's end point.
.[------]   // 1 segment, gap (missing beacon) is at the start. Get only segment's start point.
[--].[--]   // 2 segments, gap (missing beacon) is in the middle. Get first segment's end point + 1.
```

The last trick to part two is that the tuning frequency can overflow a 64-bit number.
To handle this, I delayed multiplication until I'd found both coordinates.
Then a standard BigInteger can do the calculation, and since its the final step it only needs to run once.
The performance overhead is absolutely negligible in light of the 4000000 iterations.