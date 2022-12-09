# Day 8: Treetop Tree House

I designed an optimized solution for this, but ran into an issue and decided to fall back on the brute-force approach instead.

The only trick that made it into the final solution was to skip parsing the input.
Its possible to quickly determine the grid size by finding the index of the first \r or \n character.
The index of that character will be equal to the grid dimensions.

With more more piece of information, its possible to index into any point in the grid without parsing the input or even splitting into lines. 
Starting at the previously-found index, search forward until you find a character that is not \n.
That new index will be equal to the number of characters that needs to be skipped for each row.

With both pieces of information, its possible to get the height of this tree using the algorithm `input[(row * rowSkip) + col] - 'a'`.
No parsing step needed!