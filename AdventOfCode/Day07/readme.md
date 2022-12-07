# Day 7: No Space Left On Device

For today's problem, I cheated a little bit by ignoring the `ls` command and treating all non-command output as a file listing.
Then all I had to do was maintain a reference to the "current working directory" and build out the filesystem tree line-by-line.
I created two custom classes `File` and `Directory` which are exactly what they sound like.
File contains a name and size, while directory contains a name and name->object mappings for all immediate subfolders and files.

I also took the lazy route and recursively walked the entire directory tree to get the size.
My plan was to cache or dynamically compute the value for part 2, but there was no need so I left it as-is.
There's a bit of irony there because I started today's problem with high-performance string parsing code based on `Span<T>`, and all of that went completely to waste. 

I got tripped up today because I assumed that `ReadOnlySpan<T>.operator==` was overridden to compare contents, when in fact it only checks that both spans reference the same chunk of memory.
Its my fault for not reading the documentation, but it is pretty confusing because there's an implicit cast between `string` and `ReadOnlySpan<char>`.
IMO its an easy mistake to make and there should be a compiler or IDE warning about it.
The correct way to compare a string to a span, by the way, is to call `MemoryExtensions.Equals(ReadOnlySpan<char>, ReadOnlySpan<char>, StringComparison)`.

I'm loving the raw performance possible with `Span<T>`, but *boy* is it not trivial to use correctly.
I discovered today that while there are built-in `ReadOnlySpan<char>.Trim*` extensions, there is no built-in way to split a span by a delimiter.
I ended up implementing my own `SpanExtensions.SplitLines()` extension that can split a span into lines by either `CRLF` or just `LF`.
It was fairly tricky to get right, even with some existing code to reference, but I enjoyed the challenge and now I have a faster alternative to `string.Split()`.
