namespace AdventOfCode.Day03;

public class Day03Part2 : Day03
{

    protected override void RunDay3(IEnumerable<int[][][]> groups)
    {
        var badgePrioritySum = groups
            .Aggregate(0, (sum, group) => sum + AllPriorities
                .First(priority => group
                    .All(sack => sack
                        .Any(comp => comp[priority] > 0)
                    )
                )
            );
        
        Log($"The total priority of all badges types is [{badgePrioritySum}].");
    }
}