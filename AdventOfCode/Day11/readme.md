# Day 11: Monkey in the Middle

I had to get help for part 2, and I still don't really understand it.
I get why modulus prevents the worry level from increasing, but why doesn't it affect the results?
Why do we use the LCM of the divisors?
This works, but I don't understand why.

Other than that, the solution is straightforward.
I used a single giant regular expression to parse the input, and handled the "operation" by storing a lambda function as a `Func<ulong, ulong>` in each Monkey object.
Its just nested loops from there.