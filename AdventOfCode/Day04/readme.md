# Day 4: Camp Cleanup

These elves really need to hire an expedition planner.
They're only four days into a *jungle expedition* and so far they have YOLOed their food supply, arranged their camp at random, mis-packed their rucksacks, lost their ID badges, and now mis-allocated resources by assigning multiple elves to clean the same areas.
This is going to be a disaster if they don't shape up quick.

Anyway, today's challenges were pretty straightforward.
I was only tripped up by the discovery that .NET's Regular Expression engine does NOT consider `\r\n` to be a line terminator.
To match an entire line in a windows-compatible way, you have to structure your regex like this: `^your_regex_here\r?$`.
Without the `\r?`, the `$` anchor will match skip over `\r` to match `\n` and the carriage return will become part of the match.
If your regular expression happens to not match `\r` (such as for example - this regex that I tried to use on today's input: `^(\d+)-(\d+),(\d+)-(\d+)$`), then the entire regex will just **silently fail**.
Thanks, Microsoft.
Very helpful.

I only used one trick, which was to pre-sort the elves within each pair so that the one with more work would always be first.
That allowed part 1 to be simplified by only checking `Shorter is inside Larger` instead of something like `First is inside Second or Second is inside First`.