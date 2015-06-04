# Maze Escape

## Synopsis
Endless maze game based on user reaction aimed to get a constantly moving object towards a subsequent escape by defining its direction.
## GamePlay
### Basic concept
A maze of certain size with one exit point is generated.
User defines the direction of a constantly moving arrow, which is placed into the maze. 
If the arrow hits a wall - the game is lost.
If the arrow reaches the exit - maze is regenerated.
### Key features

**Complexity and focus.** Although the maze is relatively small, the fact that the arrow starts moving instantly (or after a short period of time after maze generation) doesn’t allow the user to look through and find the optimal path, as he needs to focus on the arrow.

**Competitivity.** 
User performance is measured by score, which is appended each time he passes a cell. Its also altered when exit is reached depending how different from optimal his path was.

**Graphical style.** 
Strict geometric shapes with a rough texture.

## Special cells
### Speedup area. 
Looks like a chevron on 2 or more lined up and merged cells. Makes the movement in the right direction twice faster and twice slower in the opposite.
### Rotator. 
When the arrow passes through rotator, its direction gets appended by 90 degrees in rotator direction.
### Spring. 
Makes a move in current direction ignoring the wall.
Score modifier. Adds or subtracts (?) score when passing through.

## Implementation
## Technology
## Screens
