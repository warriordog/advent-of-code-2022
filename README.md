# advent-of-code-2022

My solutions to the [2022 Advent of Code challenge](https://adventofcode.com/).
I'm not competing on any leaderboards this year, but I do plan to complete both parts of all days.
As usual, I'm also prioritizing readable code over compact or "magic" solutions.
Feedback and questions are welcome!

### Instructions
* To run a single solution:
  * From Rider - Select a day/part from the Run Configurations dropdown and click Run.
  * From CLI - navigate to the solution root and execute `dotnet run --project AdventOfCode run Day<number> Part<number> [solution-specific arguments]`
    * Day/part numbers are case-sensitive. Day must be a two digits and part should be one digit. For example: `... Day01 Part1` would execute day 1 / part 1.
    * Any arguments after the part number will be passed on to the solution. See the "Arguments" column in the table below to see what arguments are supported.
    * Please be sure to run from the solution root directory, otherwise the paths will not resolve correctly.
* To run a single solution with test data:
  * From Rider - click "edit configurations" and select a day/part run configuration. In the "program arguments" field, change `run` to `test`.
  * From CLI - use same steps as before, but replace `run` with `test` in the command.
* To run all solutions:
  * From Rider - Select the `Run All` run configuration and click Run.
  * From CLI - navigate to the solution root and execute `dotnet run --project AdventOfCode all`


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