# Day 9: Rope Bridge

For today's problem, I took the opportunity to port a data structure from my a previous year's AoC solutions - `Axis<T>`.
Like an array or List, Axis stores data by integer index.
Unlike those structures, however, Axis allows indexes to be negative.
By nesting two Axes into a `Plane<T>`, it is possible to reference data by a coordinate in 2-dimensional cartesian space.
Axis worked well for the standard inputs, but its quite memory-heavy so I probably should have just used a HashMap instead.