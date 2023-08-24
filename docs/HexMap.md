# HexMap Library

HexMap.cs is static class which holds methods about coordinates, coordinate systems and other.

## Source

I used this website and its code to handle hex coordinates systems https://www.redblobgames.com/grids/hexagons/

## Coordinates systems

### Offset system oddq

(see website) is used by Unity Hex Grid and uses columns and rows to access the tiles.

### Axial Hex Coordinates

(see website) is used by the program to calculate adjacent tiles, because it is easier to navigate in it. It uses x and y coordinates.

## Pathfinding

The method **FindDistances()** is used to find reachable tiles by an unit. It uses Dijkstra algorithm to find the tiles. The only derivation to the basic algorithm is the feature of controlled tiles by enemy units. Therefore the algorithm needs to check if the tile is controlled and if yes then to not continue to adjacent tiles.