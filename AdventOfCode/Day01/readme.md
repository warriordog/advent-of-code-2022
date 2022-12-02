# Day 1: Calorie Counting

There's not much to say here - the solution is straightforward.
No fancy tricks, just split the input into a jagged array and then `Aggregate()` into a flat array of ints.
Part 1 reduces this with a simple call to `Max()`, and part 2 just sorts the list and pairs `.Take(3)` with another `Aggregate()` to get the final answer.
I initially misread the instructions and thought that the _index_ of the elf with the most calories was required.
This led me to introduce an unnecessary `Elf` record type to track calories and index together.
