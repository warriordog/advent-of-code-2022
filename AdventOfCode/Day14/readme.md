# Day 14: Regolith Reservoir

Yesterday, I said that day 13 was my worst-performing solution.
I was wrong, because day 14 part 2 takes ~560 ms per run.
90% of that is hashtable lookups because I decided to be lazy and store positions in a Dictionary.
I may return to this later and try to implement a custom data structure.
Since the vertical height doesn't change in either part, it should be possible to do something like `Axis<Matter[]>` to track everything in a more performant way.

I am fairly proud of this solution though, especially the `DropSandFrom` method.
I spent a lot of time making it readable despite the branching logic, and I managed to avoid recursion as well.
I'm only unhappy that I couldn't avoid repeating this three times:
```csharp
if (data[point] == Matter.Void) return false;
if (data[point] == Matter.Air) continue;
```

I really wanted to factor that out into a single check for each potential landing point, but I couldn't do it without either making the rest of the loop ugly or resorting to recursion.

Some potential optimizations:
* Faster data structure (as described above)
  * Alternately, just hardcode the width because the slope is 1. Then we can use `Matter[height*2, height]` for efficient access.
* Alternate search algorithm (modified BFS?)
* Instead of picking one left/down/right branch, run ALL of them and then there's no need for the outer loop. This also reduces duplicate work.
* Clamp search bounds to a triangle peaking at (500, 0)
* Form triangle first and ray trace in from the edges. Probably works for part 2 only.