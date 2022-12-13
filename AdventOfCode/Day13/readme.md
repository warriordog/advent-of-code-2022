# Day 13: Distress Signal

I decided to be lazy today and just parse the input as JSON.
I implemented a class called PacketValue along with a polymorphic JsonConverter to decode it from either an integer value or an array of PacketValues.
This was my first time working with `System.Text.Json`, and it was a pretty nice experience.
It wasn't nearly as limited as I've been led to believe and the documentation is vastly superior to `Newtonsoft.JSON`.

I implemented a few additional tricks to simplify the solution:
1. In part 1, I hardcoded the main loop to read two lines at a time and process them as a pair. I allowed the `while (inputLines.MoveNext());` line to both move to the next line and *also* skip over the empty dividing line.
2. I implemented `IComparable<PacketValue>` so that I could use normal .NET sorting functions to sort and compare packets. Part 1 calls `compareTo()` directly and in part two it enables the use of `List<PacketValue>.Sort()` with no need for a custom comparer.
3. Part 2 stores a reference to the generated divider packets so that finding their index is just a simple `packets.IndexOf(divider)`. That avoided the need to somehow check each packet to see whether or not its a divider.
