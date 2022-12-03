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
  2. Execute `dotnet run --project AdventOfCode -- <command> [options]`
     * Supported commands:
       * `run <day> [part] [--input path_to_input]` - run one or more solutions. Part defaults to `all`.
       * `list [day] [part]` - list available solutions. Part and day default to `all`.
       * `help [command]` - show help.
       * `version` - show project version.
     * Parameters:
       * `day` - should be in `Day##` format. Can also be the string `all` to select all days.
       * `part` - should be in `Part#` format. Can also be the string `all` to select all parts.
       * `path_to_input` - if set, overrides the input file. Path is resolved relative to the current working directory.
       * `command` - if set, shows detailed help about a specific command.

### Solutions
| Day                         | Part 1                                    | Part 2                                    | Name                    | Arguments |
|-----------------------------|-------------------------------------------|-------------------------------------------|-------------------------|-----------|
| [Day03](AdventOfCode/Day03) | [Part1](AdventOfCode/Day03/Day03Part1.cs) | [Part2](AdventOfCode/Day03/Day03Part2.cs) | Rucksack Reorganization | n/a       |
| [Day02](AdventOfCode/Day02) | [Part1](AdventOfCode/Day02/Day02Part1.cs) | [Part2](AdventOfCode/Day02/Day02Part2.cs) | Rock Paper Scissors     | n/a       |
| [Day01](AdventOfCode/Day01) | [Part1](AdventOfCode/Day01/Day01Part1.cs) | [Part2](AdventOfCode/Day01/Day01Part2.cs) | Calorie Counting        | n/a       |

### Details
* Dotnet 6 is required to run the solutions.
* Project files are included for JetBrains Rider, but the solution should work in Visual Studio or with the dotnet command line.
* Solutions should run on any supported .NET platform.