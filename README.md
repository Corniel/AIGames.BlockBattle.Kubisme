Kubisme
=======

Kubisme (Cubism) is an AI trying to play Tetris.

Version 32
----------
Fix in path generation for long (30+) paths. Added T-Spin detection and 
parameters.

Version 31
----------
Fix in path finding. added T-spin point. Changed evaluation; check for free 
rows, instead of free cells.

Version 30
----------
Last blockade detection and path finding for filling holes.

Version 29
----------
Disabled combo bonus for rows of 9.

Version 28
----------
Tweaked parameters based on the simulation fix.

Version 27
----------
Yet another time consumption tweak.

Version 26
----------
Fix in time consumption.

Version 25
----------
Tweaked branching factor based on available blocks, potentially leading to two
ply more.

Version 24
----------
Re-factoring if the block object. Different implementation handle block type
specific logic, including a test on the availability of the path.

Version 23
----------
New parameters and a block node lock row fix.

Version 22
----------
Max depth to 5, and higher branching, to prevent time-outs.

Version 21
----------
Fix in add garbage rows.

Version 20
----------
Bug fixes and testing added. New live data needed.

Version 19
----------
Clearing fix, extended logging.

Version 18
----------
Disabled filling (for now), implemented new rules.

Version 17
----------
Fix in starting position, first taken in to account in Version 16.

Version 16
----------
Further tweaked the parameters. Reduced the branching factor even further. On top 
of that, Introduced path finding for filling (reachable, duh) holes.

Version 15
----------
Introduced a rudimental combo potential detection, and tweaked (increased) the
branching factor a bit. Based on a stronger virtual opponent and the compo 
potential, updated parameters as well.

Version 14
----------
Fixed the determination of blockades, and of the the contacts with the right 
wall. Furthermore, the parameters were updated. On top of that, the search
depth is extended. In practice, it searches up to 6 ply.

Version 13
----------
Failed to compile on TheAIGames.com due to some not supported reflection code?!

Version 12
----------
Added block per row count specific weights.

Version 11
----------
Changed the genetics for optimizing evaluation parameters. With improved 
parameters and more checked variations.

Version 10
----------
Updated evaluation parameters. (Not that successful).

Version 9
---------
Fixed the swapped L rotate left and right.

Version 8
---------
Fixed rotate right bug (was move right).

Version 7
---------
Fixed a lot of bugs, enabled evaluation of one 'unknown' block as well.

Version 1
---------
First attempt. Local runs (without competition) run 75 points on average before dying.