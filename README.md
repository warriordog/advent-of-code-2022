# advent-of-code-2022

My solutions to the [2022 Advent of Code challenge](https://adventofcode.com/).
I'm not competing on any leaderboards this year, but I do plan to complete both parts of all days.
As usual, I'm also prioritizing readable code over compact or "magic" solutions.
Feedback and questions are welcome!

### Usage
* From Rider IDE:
  1. select one of the included run configurations and click "Run" or "Debug".
* From a terminal:
  1. Open a terminal and navigate to the solution root.
  2. Execute `dotnet run --configuration Release --project Main -- <command> [options]`
     * Supported commands:
       * `list <what>` - list data from internal registries. `what` specifies what data to return and can be one of these options:
         * `solutions [day] [part]` - list available solutions. `day` and `part` both default to `all`.
         * `inputs [day] [part] [variant]` - list registered input files for a solution. `day`, `part`, and `variant` all default to `all`.
       * `run [day] [part] [variant] [options]` - run one or more solutions. `day` and `part` both default to `all`. `variant` defaults to null, which means no variants will be selected. Supports options:
         * `[--input {id | name | type}]` - select a registered input file to use. Use `list inputs` to show all options. Applies to all selected days/parts.
         * `[--custom-input path_to_input]` - specify an custom, external input file to use. Applies to all selected days/parts.
       * `bench [day] [part] [variant] [options]` - benchmark one or more solutions. Supports same options and defaults as `run`, and additionally supports:
         * `[--min-warmup-time time_in_ms]` - set the minimum time (in milliseconds) to run warmup rounds (default 2000ms).
         * `[--min-warmup-rounds num_rounds]` - set the minimum number of warmup rounds (default 10).
         * `[--min-sample-time time_in_ms]` - set the minimum time (in milliseconds) to run sampling (benchmark) rounds (default 10000ms).
         * `[--min-sample-rounds num_rounds]` - set the minimum number of sampling (benchmark) rounds (default 10).
         * `[--no-warmup]` - skip warmup rounds entirely. Useful when using an external profiler.
       * `--help [command]` - show help.
       * `--version` - show project version.
     * Options:
       * `--vebose` - Show verbose / debug output. Defaults to off.
     * Parameters:
       * `day` - should be in `Day##` format. Can also be the string `all` to select all days.
       * `part` - should be in `Part#` format. Can also be the string `all` to select all parts.
       * `variant` - format is day/part-specific. Can also be the string `all` to select all variants.
       * `path_to_input` - if set, overrides the input file. Path is resolved relative to the current working directory.
       * `command` - if set, shows detailed help about a specific command.

### Solutions
| Day                         | Part 1                                         | Part 2                                         | Name                                                            |
|-----------------------------|------------------------------------------------|------------------------------------------------|-----------------------------------------------------------------|
| [Day16](AdventOfCode/Day16) | [Part1](AdventOfCode/Day16/Day16Part1.cs)      | *incomplete*                                   | [Proboscidea Volcanium](https://adventofcode.com/2022/day/16)   |
| [Day15](AdventOfCode/Day15) | [Part1](AdventOfCode/Day15/Day15Part1.cs)      | [Part2](AdventOfCode/Day15/Day15Part2.cs)      | [Beacon Exclusion Zone](https://adventofcode.com/2022/day/15)   |
| [Day14](AdventOfCode/Day14) | [Part1](AdventOfCode/Day14/Day14NaivePart1.cs) | [Part2](AdventOfCode/Day14/Day14NaivePart2.cs) | [Regolith Reservoir](https://adventofcode.com/2022/day/14)      |
| [Day13](AdventOfCode/Day13) | [Part1](AdventOfCode/Day13/Day13Part1.cs)      | [Part2](AdventOfCode/Day13/Day13Part2.cs)      | [Distress Signal](https://adventofcode.com/2022/day/13)         |
| [Day12](AdventOfCode/Day12) | [Part1](AdventOfCode/Day12/Day12Part1.cs)      | [Part2](AdventOfCode/Day12/Day12Part2.cs)      | [Hill Climbing Algorithm](https://adventofcode.com/2022/day/12) |
| [Day11](AdventOfCode/Day11) | [Part1](AdventOfCode/Day11/Day11Part1.cs)      | [Part2](AdventOfCode/Day11/Day11Part2.cs)      | [Monkey in the Middle](https://adventofcode.com/2022/day/11)    |
| [Day10](AdventOfCode/Day10) | [Part1](AdventOfCode/Day10/Day10Part1.cs)      | [Part2](AdventOfCode/Day10/Day10Part2.cs)      | [Cathode-Ray Tube](https://adventofcode.com/2022/day/10)        |
| [Day09](AdventOfCode/Day09) | [Part1](AdventOfCode/Day09/Day09Part1.cs)      | [Part2](AdventOfCode/Day09/Day09Part2.cs)      | [Rope Bridge](https://adventofcode.com/2022/day/9)              |
| [Day08](AdventOfCode/Day08) | [Part1](AdventOfCode/Day08/Day08Part1.cs)      | [Part2](AdventOfCode/Day08/Day08Part2.cs)      | [Treetop Tree House](https://adventofcode.com/2022/day/8)       |
| [Day07](AdventOfCode/Day07) | [Part1](AdventOfCode/Day07/Day07Part1.cs)      | [Part2](AdventOfCode/Day07/Day07Part2.cs)      | [No Space Left On Device](https://adventofcode.com/2022/day/7)  |
| [Day06](AdventOfCode/Day06) | [Part1](AdventOfCode/Day06/Day06Part1.cs)      | [Part2](AdventOfCode/Day06/Day06Part2.cs)      | [Tuning Trouble](https://adventofcode.com/2022/day/6)           |
| [Day05](AdventOfCode/Day05) | [Part1](AdventOfCode/Day05/Day05Part1.cs)      | [Part2](AdventOfCode/Day05/Day05Part2.cs)      | [Supply Stacks](https://adventofcode.com/2022/day/5)            |
| [Day04](AdventOfCode/Day04) | [Part1](AdventOfCode/Day04/Day04Part1.cs)      | [Part2](AdventOfCode/Day04/Day04Part2.cs)      | [Camp Cleanup](https://adventofcode.com/2022/day/4)             |
| [Day03](AdventOfCode/Day03) | [Part1](AdventOfCode/Day03/Day03Part1.cs)      | [Part2](AdventOfCode/Day03/Day03Part2.cs)      | [Rucksack Reorganization](https://adventofcode.com/2022/day/3)  |
| [Day02](AdventOfCode/Day02) | [Part1](AdventOfCode/Day02/Day02Part1.cs)      | [Part2](AdventOfCode/Day02/Day02Part2.cs)      | [Rock Paper Scissors](https://adventofcode.com/2022/day/2)      |
| [Day01](AdventOfCode/Day01) | [Part1](AdventOfCode/Day01/Day01Part1.cs)      | [Part2](AdventOfCode/Day01/Day01Part2.cs)      | [Calorie Counting](https://adventofcode.com/2022/day/1)         |

### Details
* Dotnet 6 is required to run the solutions.
* Project files are included for JetBrains Rider, but the solution should work in Visual Studio or with the dotnet command line.
* Solutions should run on any supported .NET platform.