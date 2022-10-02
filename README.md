# Senku Peg solitaire
## The Game
Simple recreation of an old board game I always enjoyed.
It is a one player game involving movement of pegs on a board with holes.
The objective is, making valid moves, to empty the entire board except for a solitary peg in the central hole.
A valid move is to jump a peg orthogonally over an adjacent peg into a hole two positions away and then to remove the jumped peg.
## The Project
The project uses an event approach with a Game Facade to manage the flow.
It has Undo/Redo functinality, a simple save/load system and UI tooltips and win condition.
