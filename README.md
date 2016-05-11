Kubisme
=======

Kubisme (Cubism) is an AI trying to play Tetris.

Version 79
----------
Parameters based on 3-ply simulations and a bugfix in lock row.

Version 78
----------
Taking opponent first filled into account did not pay off. So removed.

Version 77
----------
SingleEmpties not longer commented out (ouch!) New parameters. Fix in finding
skips, and finding paths on ply 2 and deeper.

Version 76
----------
Overflow fix on opponent.

Version 75
----------
Take first filled of the opponent into account. Removed evaluation of tunnels
and other unreachable potential. Is seems not to work.

Version 74
----------
New parameters.

Version 73
----------
New way of handling curves in parameter generation.

Version 72
----------
Always check on unreachable holes. Check paths in endgame. Added evaluation for to
penalize unreachable rows with potential.

Version 71
----------
Take height of unreachables into account when valuation unreachables.

Version 70
----------
Rolled 3ply stuff back too.

Version 69
----------
Rolled back to Version 65 with 'Improved speed of path finding enormously' of
Version 66.

Version 68
----------
New parameters fine-tuned on 3 ply.

Version 67
----------
New parameters and exclude delta score for more than 3 ply.

Version 66
----------
Increased opponent analysis to 3 ply. Only search for I-potential with 'deep tunnels'.
Take first filled of opponent into account for the score of a field. Improved 
speed of path finding enormously.

Version 65
----------
Extended curved parameters.

Version 64
----------
Introduced curved parameters, and single T-spin detection. Score 0.67.

Version 63
----------
Divide by zero exception bug fix for winning on ply 2 and 1, and losing on the next turn.

Version 62
----------
Fixes for bugs with getting T-spin bonus (resulting in way to high scores). Added 
all 7 blocks for ply 3. Score 0.60.

Version 61
----------
Parameters with a score 0.74.

Version 60
----------
With parameters of version 56.

Version 59
----------
Fixed endgame evaluation, improved T-spin detection. New parameters, and
unreachables count as two holes per line. 

Version 58
----------
For now, only 2 ply deep. Tweak in factor for unreachables.

Version 57
----------
Hopefully improved parameters.

Version 56
-----------
Split parameters in default and endgame, and added second ply for opponent evaluation.

Version 55
----------
Rolled back the parameter changes, as they performed shit.

Version 54
----------
Fixed a painful block path finding bug. Updated parameters because of this bug.

Version 53
----------
Added penalties for single empties.

Version 52
----------
Send the last move that was found, not the be losing directly, and an update of the logging.

Version 51
----------
Re-introduced garbage in the evaluation, using the worst outcome of the current
block, and detecting potential win on the current block.

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
