# Day 2: Rock Paper Scissors

This solution uses lookup tables and static enumerations to abstract the logic of Rock, Paper, Scissors.
That ended up being a complete waste, but it seemed smart at the time because I was fully confident that part 2 would introduce "Lizard" and "Spock".
Alas, it did not, so I ended up with an over-engineered solution based on a circular directed graph of `Move` objects.
Each object contains a reference to the Move that it wins against and the move that it loses to.
Any move can be compared against any other move to produce an `Outcome`, which is an enumeration of `Win`, `Lose`, or `Draw`.
Each type of Outcome has an associated point value which is used to compute a round's score.
The input is parsed into a list of `Round` objects, which contain each player's moves and encapsulate the logic to score a round.
Once the data structure is built, the total score can be computed with a single `.Aggregate()` call over `round.GetPlayerScore()`.
Over-engineered? Yes.
Inefficient? Also yes.
But its effective, extensible, and the primary logic is cleanly abstracted for easy reading.

**Observations**
* It turns out that its not easy to initialize such a circular structure in null-safe C#, so I had to fall back on a static class initializer and heavy use of the null-suppressing operator. Eww.
* The logic between rounds ended up being so similar that I was able to abstract almost all of it. The per-part code is limited to solving X/Y/Z and formatting the output message.