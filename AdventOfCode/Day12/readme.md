# Day 12: Hill Climbing Algorithm

I initially solved this using reverse Dijkstra's algorithm, which is just Dijkstra but starting from the end point.
Running in reverse means that the output contains the shortest paths for *all* nodes instead of just the starting point, which makes part 2 trivial.
My parsing code is convoluted and honestly not that great, because I tried to avoid creating an actual tree structure.
Instead, I have a thin wrapper around a 2D array that contains non-linked nodes.
I think the solution would be cleaner and more performant if it parsed into a proper graph instead.

After completing the solution, I realized that Dijkstra was overkill and that there was probably a simpler algorithm.
Breadth-first search fit the bill, so I converted the solution to use that.
Benchmarking both versions showed that BFS was about 2x as fast as Dijkstra's, while also being simpler and easy to understand.
Definitely the better choice here!