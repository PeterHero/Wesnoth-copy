# Grid Manager

GridManager.cs is responsible for generating map of tiles, tile coloring, unit movement and handlick tile input - that is **TileHovered()** and **TileClicked()**.

**TileHovered()** and **TileClicked()** contain a decision tree based on what is clicked, what was focused before and so on. The click method triggers unit movement as well as a battle screen with an attacking and a defending unit.