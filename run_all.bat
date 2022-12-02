@echo off

echo Running all solutions...
dotnet build > NUL
dotnet run --no-build --project AdventOfCode Day01 Part1
dotnet run --no-build --project AdventOfCode Day01 Part2
dotnet run --no-build --project AdventOfCode Day02 Part1
dotnet run --no-build --project AdventOfCode Day02 Part2