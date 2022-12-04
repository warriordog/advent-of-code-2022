# advent-of-code-2022

My solutions to the [2022 Advent of Code challenge](https://adventofcode.com/).
I'm not competing on any leaderboards this year, but I do plan to complete both parts of all days.
As usual, I'm also prioritizing readable code over compact or "magic" solutions.
Feedback and questions are welcome!

### Usage
* From Rider IDE:
  1. select one of the included run configurations and click "Run" or "Debug".
* From a terminal:
  1. Open a terminal and navigate to the solution root. Please be sure to run from the solution root and not the project root or build directory. Otherwise the paths will not resolve correctly.
  2. Execute `dotnet run --configuration Release --project AdventOfCode -- <command> [options]`
     * Supported commands:
       * `list [day] [part]` - list available solutions. `part` and `day` both default to `all`.
       * `run <day> [part] [options]` - run one or more solutions. Part defaults to `all`. Supports options:
         * `[--input path_to_input]` - specify an alternate input file to use. Applies to all selected days/parts.
       * `bench <day> [part] [options]` - benchmark one or more solutions. Supports same options and defaults as `run`, and additionally supports:
         * `[--min-warmup-time time_in_ms]` - set the minimum time (in milliseconds) to run warmup rounds (default 2000ms).
         * `[--min-warmup-rounds num_rounds]` - set the minimum number of warmup rounds (default 10).
         * `[--min-sample-time time_in_ms]` - set the minimum time (in milliseconds) to run sampling (benchmark) rounds (default 10000ms).
         * `[--min-sample-rounds num_rounds]` - set the minimum number of sampling (benchmark) rounds (default 10).
       * `--help [command]` - show help.
       * `--version` - show project version.
     * Parameters:
       * `day` - should be in `Day##` format. Can also be the string `all` to select all days.
       * `part` - should be in `Part#` format. Can also be the string `all` to select all parts.
       * `path_to_input` - if set, overrides the input file. Path is resolved relative to the current working directory.
       * `command` - if set, shows detailed help about a specific command.

### Solutions
| Day                         | Part 1                                    | Part 2                                    | Name                                                           |
|-----------------------------|-------------------------------------------|-------------------------------------------|----------------------------------------------------------------|
| [Day04](AdventOfCode/Day04) | [Part1](AdventOfCode/Day04/Day04Part1.cs) | [Part2](AdventOfCode/Day04/Day04Part2.cs) | [Camp Cleanup](https://adventofcode.com/2022/day/4)            |
| [Day03](AdventOfCode/Day03) | [Part1](AdventOfCode/Day03/Day03Part1.cs) | [Part2](AdventOfCode/Day03/Day03Part2.cs) | [Rucksack Reorganization](https://adventofcode.com/2022/day/3) |
| [Day02](AdventOfCode/Day02) | [Part1](AdventOfCode/Day02/Day02Part1.cs) | [Part2](AdventOfCode/Day02/Day02Part2.cs) | [Rock Paper Scissors](https://adventofcode.com/2022/day/2)     |
| [Day01](AdventOfCode/Day01) | [Part1](AdventOfCode/Day01/Day01Part1.cs) | [Part2](AdventOfCode/Day01/Day01Part2.cs) | [Calorie Counting](https://adventofcode.com/2022/day/1)        |

### Details
* Dotnet 6 is required to run the solutions.
* Project files are included for JetBrains Rider, but the solution should work in Visual Studio or with the dotnet command line.
* Solutions should run on any supported .NET platform.