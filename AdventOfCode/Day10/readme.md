# Day 10: Cathode-Ray Tube

I got majorly tripped up by failing to realize that the X register doesn't update until *both* cycles of `addx` are completed.
After finally decoding what was meant by this part of the instructions:
> During the 20th cycle, register X has the value 21, so the signal strength is 20 * 21 = 420. (The 20th cycle occurs in the middle of the second addx -1, so the value of register X is the starting value, 1, plus all of the other addx values up to that point: 1 + 15 - 11 + 6 - 3 + 5 - 1 - 8 + 13 + 4 = 21.)

I was able to identify and fix the issue.

My solution is a bit "inverted" from a typical approach, because I didn't want to implement a full simulated CPU pipeline.
Instead of looping through each cycle and fetching new instructions as-needed, I instead loop through the *instructions* and execute *cycles* as-needed.
I chose this approach because I didn't want to bother with a "better" approach like delaying the instruction or having multiple copies of registers.
Instead, I simply inserted the cycles after the instruction is decoded, but before the register is updated:
```csharp
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
```
This worked quite well and would easily scale to include new instructions with varying execution times.