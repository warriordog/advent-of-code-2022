namespace AdventOfCode.Day03;

public class Day03Part1 : Day03
{
    protected override void RunDay3(IEnumerable<int[][][]> groups)
    {
        var conflictPrioritySum = groups
            .SelectMany(group => group)
            .Aggregate(0, (sum, sack) => sum + AllPriorities
                .First(priority => sack
                    .All(comp => comp[priority] > 0)
                )
            );
        
        Log($"The total priority of conflicting items is [{conflictPrioritySum}].");
    }
}