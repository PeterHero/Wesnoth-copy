# Programming documentation - crossroad

The core of the project are scripts that can be found in **/Assets/Scripts** folder.

There is more than one type of scripts. Managers, object classes and a 'library'. 

## Managers

- Battle manager
- Game manager
- Grid manager
- UI Manager

These are controllers of the game. They handle user input and game logic.

## Object classes

- Attack
- Player
- Tile
- Unit
- Village

These are scriptable objects (Tile, Unit, Village) or other (Attack, Player)

## Libraries

- Hexmap

These are static classes which hold helper methods of one topic.

## Notes

Some class variables are variables and some are properties and it may seem random. It is this way because properties are not visible in the unity editor and therefore are used to declutter the editor of uneditable variables.