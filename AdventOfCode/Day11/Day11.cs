using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day11;

[InputFile("input.txt")]
[InputFile("test.txt", InputFileType.Test)]
public abstract class Day11 : ISolution
{
    // Regex abuse
    private readonly Regex _parseMonkeyRegex = new(@"Monkey (\d)+:\s+Starting items: (\d+(?:, \d+)*)\s+Operation: new = old ([*+]) (\d+|old)\s+Test: divisible by (\d+)\s+If true: throw to monkey (\d+)\s+If false: throw to monkey (\d+)", RegexOptions.Compiled);
    
    private readonly int _roundCount;
    private readonly ulong _roundDivisor;
    private readonly ILogger _logger;
    
    protected Day11(ILogger logger, int roundCount, ulong roundDivisor = 1ul)
    {
        _roundCount = roundCount;
        _roundDivisor = roundDivisor;
        _logger = logger;
    }

    public void Run(string inputFile)
    {
        var monkeys = ParseMonkeys(inputFile);
        if (monkeys.Count < 2)
        {
            throw new ArgumentException("Input file is invalid - there must be at least two monkeys", nameof(inputFile));
        }
        
        // Compute LCM to use in modulus below.
        // This is needed to limit growth of worry level.
        var modulo = monkeys.Aggregate(1ul, (prod, m) => prod * m.TestDivisor);
        
        // Run each round
        for (var round = 0; round < _roundCount; round++)
        {
            // Give each monkey a turn
            foreach (var monkey in monkeys)
            {
                // Inspect and throw each item
                while (monkey.Items.TryDequeue(out var itemWorry))
                {
                    // Compute new worry level.
                    var newWorry = monkey.Operation(itemWorry) / _roundDivisor % modulo;   
                    
                    // Test the new worry level
                    var meetsTest = newWorry % monkey.TestDivisor == 0ul;

                    // Throw to the appropriate monkey
                    var targetMonkeyId = meetsTest ? monkey.TrueMonkey : monkey.FalseMonkey;
                    var targetMonkey = monkeys[targetMonkeyId];
                    targetMonkey.Items.Enqueue(newWorry);

                    // Update its inspection count
                    monkey.InspectionCount++;
                }
            }
        }

        var mostActiveMonkeys = monkeys
            .OrderByDescending(monkey => monkey.InspectionCount)
            .Take(2)
            .ToList();

        var activeMonkey1 = mostActiveMonkeys[0];
        var activeMonkey2 = mostActiveMonkeys[1];
        var activeMonkeyProduct = activeMonkey1.InspectionCount * activeMonkey2.InspectionCount;

        _logger.LogInformation("The two most active monkeys are {monkey1} ({monkey1Inspections} inspections) and {monkey2} ({monkey2Inspections} inspections). The product of these is [{inspectionProduct}] inspections.", activeMonkey1.Id, activeMonkey1.InspectionCount, activeMonkey2.Id, activeMonkey2.InspectionCount, activeMonkeyProduct);
    }

    private List<Monkey> ParseMonkeys(string input)
    {
        var monkeys = new List<Monkey>();
        
        // Parse each monkey
        foreach (Match match in _parseMonkeyRegex.Matches(input))
        {
            // Parse basic details
            var id = int.Parse(match.Groups[1].ValueSpan);
            var testDivisor = ulong.Parse(match.Groups[5].ValueSpan);
            var trueMonkey = int.Parse(match.Groups[6].ValueSpan);
            var falseMonkey = int.Parse(match.Groups[7].ValueSpan);

            // Parse operation
            var operationFunc = match.Groups[3].ValueSpan[0]; // Bit of a hack, but we only support a single char anyway :shrug:
            var operationValue = match.Groups[4].Value;
            var operation = ParseOperation(operationFunc, operationValue);
            
            // Parse starting items
            var startingItemList = match.Groups[2].Value
                .Split(", ")
                .Select(ulong.Parse);
            var startingItems = new Queue<ulong>(startingItemList);
            
            // Create monkey
            monkeys.Add(new Monkey(id, operation, testDivisor, trueMonkey, falseMonkey, startingItems));
        }

        if (!monkeys.Any())
        {
            throw new ApplicationException("Failed to parse any monkeys!");
        }
        
        return monkeys;
    }

    private static Func<ulong, ulong> ParseOperation(char func, string value)
    {
        if (value == "old")
        {
            return ParseUnaryOperation(func);
        }
        else
        {
            var valuenum = ulong.Parse(value);
            return ParseBinaryOperation(func, valuenum);
        }
    }

    private static Func<ulong, ulong> ParseUnaryOperation(char func) => func switch
    {
        '+' => old => old + old,
        '*' => old => old * old,
        _ => throw new ArgumentOutOfRangeException(nameof(func), $"Operation func is invalid, should be '+' or '*' but got '{func}'")
    };
    
    private static Func<ulong, ulong> ParseBinaryOperation(char func, ulong value) => func switch
    {
        '+' => old => old + value,
        '*' => old => old * value,
        _ => throw new ArgumentOutOfRangeException(nameof(func), $"Operation func is invalid, should be '+' or '*' but got '{func}'")
    };
}