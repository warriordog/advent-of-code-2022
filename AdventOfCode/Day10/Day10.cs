using System.Text;
using AdventOfCode.Common;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Day10;

[InputFile("input.txt")]
[InputFile("test1.txt", InputFileType.Test)]
[InputFile("test2.txt", InputFileType.Test)]
public class Day10 : ISolution
{

    private readonly bool _printInterestingSignals;
    private readonly bool _printDisplayOutput;
    private readonly ILogger _logger;
    protected Day10(ILogger logger, bool printInterestingSignals, bool printDisplayOutput)
    {
        _logger = logger;
        _printInterestingSignals = printInterestingSignals;
        _printDisplayOutput = printDisplayOutput;
    }

    public void Run(string inputFile)
    {
        var input = inputFile
            .AsSpan()
            .TrimEnd()
            .SplitLines();

        // These will be populated by the logic
        var totalInterestingSignalStrength = 0;
        var displayOutput = new StringBuilder();

        // CPU counters
        var xRegister = 1;
        var cycle = 1;
        
        // lol what a hack
        void RunCycle()
        {
            // Count interesting cycles
            var signalStrength = cycle * xRegister;
            if (cycle is 20 or 60 or 100 or 140 or 180 or 220)
            {
                totalInterestingSignalStrength += signalStrength;
            }
            
            // Draw the next pixel
            var crtPosition = (cycle - 1) % 40; // Cycles are 1-index
            var spriteIsVisible = (xRegister - 1) <= crtPosition && (xRegister + 1) >= crtPosition;
            displayOutput.Append(spriteIsVisible ? '█' : ' ');
            if (crtPosition == 39)
            {
                displayOutput.Append('\n');
            }
            
            // Increment cycle
            cycle++;
            
            _logger.LogDebug("After cycle {cycle}, X={xRegister} and the signal strength is {signalStrength}. The display is drawing at {crtPosition} which means that the sprite {visibility} visible.", cycle, xRegister, signalStrength, crtPosition, spriteIsVisible ? "is": "is not");
        }
        
        foreach (var line in input)
        {
            // addx instruction
            if (line[0] == 'a')
            {
                // Execute first
                RunCycle();
                RunCycle();
                
                // Then increment
                var argument = int.Parse(line[5..]);
                xRegister += argument;
            }
            
            // noop instruction
            else if (line[0] == 'n')
            {
                RunCycle();
            }

            // Invalid instruction
            else
            {
                throw new ApplicationException($"Failed to decode instruction line '{line}'");
            }
        }


        // Part1 solution
        if (_printInterestingSignals)
        {
            _logger.LogInformation("The sum of all six interesting signal strength is [{sum}].", totalInterestingSignalStrength);
        }
        
        // Part2 solution
        if (_printDisplayOutput)
        {
            _logger.LogInformation("The rendered image is:\n{displayOutput}", displayOutput);
        }
    }
}