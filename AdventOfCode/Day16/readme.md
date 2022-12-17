# Day 16: Proboscidea Volcanium

Finally.
After nearly eight hours, I got part 1 working.
It took multiple false starts and rabbit holes, but I figured out a solution that can solve part 1 in about 52 ms.
I don't know exactly *how* it works, because it was largely trial and error, but here are some of the key tricks:

* In the parsing phase, I run an initial set of breadth-first searches to compute the shortest path between any two nodes. Then I can abstract the main movement operation into a sequence of multiple moves instead of just a single move. Each step of the simulation processes all the movements between the current location and the target location in one shot.
* Incomplete paths track statistics - specifically "MinFlow" and "MaxFlow". These represent (respectively) the smallest possible and largest possible result that may occur by following this path. These can be used to as a heuristic to optimize BFS.
* The main loop tracks the best path found so far. If any intermediate path is found to have a higher MinFlow, then it becomes the best path. Conversely, if any intermediate path is found to have a MaxFlow less than the best path's MinFlow, then it is discarded.
* I created a custom `PathQueue` structure that "loosely" sorts intermediate paths based on their MinFlow. Paths with a higher MinFlow are more likely to be returned, which increases the chances that a new best path will be found. The "sorting" is literally just division into a bucket so there's minimal overhead.
* PathQueue implicitly implements the greedy algorithm because it sorts by MinFlow. When combined with BFS, this acts as a heuristic to greatly speed up the main loop. The MinFlow rapidly scales up and large portions of the search area are discarded before even being reached.