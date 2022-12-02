# advent-of-code-2022

### Instructions
To run, build the solution and pass in a day and part as arguments.
This is case-sensitive and days must be two-digit numbers.
Example: `dotnet AdventOfCode/bin/Debug/net6.0/AdventOfCode.dll Day01 Part1`.
Please be sure to run from the solution root directory, otherwise the paths will not resolve correctly.

### Solutions
| Day                         | Part 1                                    | Part 2                                    | Name                | Arguments |
|-----------------------------|-------------------------------------------|-------------------------------------------|---------------------|-----------|
| [Day01](AdventOfCode/Day01) | [Part1](AdventOfCode/Day01/Day01Part1.cs) | [Part2](AdventOfCode/Day01/Day01Part2.cs) | Calorie Counting    | n/a       |
| [Day02](AdventOfCode/Day02) | [Part1](AdventOfCode/Day02/Day02Part1.cs) | [Part2](AdventOfCode/Day02/Day02Part2.cs) | Rock Paper Scissors | n/a       |

### Details
* Dotnet 6 is required to run the solutions.
* Project files are included for JetBrains Rider, but the solution should work in Visual Studio or with the dotnet command line.
* Solutions are executed via a [reflective loader](AdventOfCode/Program.cs).
  * Day/part names are used to construct the type name, which is resolved to a type via reflection.
  * Any extra parameters are passed on to the specific solution class.
* Solutions should run on any supported .NET platform.