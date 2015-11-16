Kubisme
=======

Kubisme (Cubism) is an AI trying to play Tetris.

Version 50
----------
Just new parameters.

Version 49
----------
Added losing-changes for row 1/4, and keep that in the evaluation for depth 1.

Version 48
----------
Re-introduced combo-potential. Removed score for empty cells (not for empty rows).
Use 'locks' instead of 'garbage' to simulate less space through time (due to
actions of the opponent). Both faster and less random.

Version 47
----------
Removed unreachable distance detection, changed branching factor. Added 
detection of multiple row clearance potential.

Version 46
----------
Removed wall score, added distance to unreachable hole.

Version 45
----------
Totally re-written field evaluator.

Version 44
----------
Fixed a Skip bug. (Skip instead of Skips,drop)

Version 43
----------
Removed opponent logic, simplified the evaluator.

Version 42
----------
Applied new rules, without taking the 'skip' move into account.

Version 41
----------
Redesign of time management, and fix of evaluation of row difference.

Version 40
----------
Bug-fixes from live resolved.

Version 39
----------
Changed branching, used a fixed number of blocks for ply 3 and deeper.

Version 38
----------
Due to disk space issues on the server, this compilation failed.

Version 37
----------
Radically change in evaluation the position. Move focus on the position relative to the opponent. 

Version 36
----------
Tweaked parameters based on one line of garbage per 4 instead of 6 points.

Version 35
----------
Fix for positions of S and Z block after rotation left and right.

Version 34
----------
Detection for unreachable lines (mainly garbage) and the reachables.

Version 33
----------
Bug fix for paths of I,S, and Z blocks. Tweaked detection for T-Spin.

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