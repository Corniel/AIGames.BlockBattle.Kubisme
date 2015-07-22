Kubisme
=======

Kubisme (Cubism) is an AI trying to play Tetris.

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