Kubisme
=======

Kubisme (Cubism) is an AI trying to play Tetris.

Version 22
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