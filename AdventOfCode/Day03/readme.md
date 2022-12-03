# Day 3: Rucksack Reorganization

This one took forever because I made not one, not two, but THREE tiny logic errors in the conversion between characters and priority levels.
Its apparently possible to screw up the logic in just such a way that it works for part 1 AND the test data for part 2, but the actual input is just slightly off.
The mistakes I made were:

1. Forgot that "a" is greater than "A" in ASCII
2. Entered `> 'a'` instead of `>= 'a'`
3. Mapped 'a' to priority 27 and 'A' to priority 1, instead of the other way around.

Somehow, these errors managed to never overflow any array bounds so nothing ever crashed.
It just produced the wrong answers.
Quite annoying.

Besides that, however, the solution was pretty straightforward.
I first split the input into lines, then grouped them into 3-element arrays.
Each array element represents one rucksack/elf, and contains two nested arrays each representing a compartment.
The compartment consists of yet another nested array, where each index is a priority level and the value is the number of items of that priority that are contained in the compartment.
Storing the data in this way made it trivial to search for matches, because I can just loop from 1 to 52 and check each sack/compartment.
The entire data model for both parts is just `IEnumerable<int[][][]>`.
This simplified the implementation of both parts and also means that the solution could be easily expanded to support any number of elves per group or compartments per sack.

That abstraction made the part-specific logic almost trivially simple
Each one was just a LINQ expression over the parsed data structure produced by the common code.

For part one, that expression is:
```csharp
groups
    .SelectMany(group => group)
    .Aggregate(0, (sum, sack) => sum + AllPriorities
        .First(priority => sack
            .All(comp => comp[priority] > 0)
        )
    )
```

Part two was only marginally more complex:
```csharp
groups
    .Aggregate(0, (sum, group) => sum + AllPriorities
        .First(priority => group
            .All(sack => sack
                .Any(comp => comp[priority] > 0)
            )
        )
    )
```