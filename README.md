# Wesnoth-copy

A project that is a copy of an open-source game Battle for wesnoth: https://www.wesnoth.org/

This project does not take any credit for the idea or graphics used in the game.

This project is created by Petr Hrdina as a semestral project for class Programming 2 on MFF CUNI.

## How to play the game

The game has 2 players that take turns. The player on turn can move their units and attack opponents unit. Players can also recruit units by pressing 'r' while the hero of the player is on the tile he was when the game started. Units can attack each other when next to each other - click on your unit and then on the opponent's adjacent unit to open to attack screen.

The game ends when one of the heroes dies.

## Game mechanics

### Income

Each player has starting gold and base income of $2. Each captured village increases the income by another $2.

### Villages

A special building. Can be captured by ending movement with player's unit. Unit's MP (movement points) goes to 0 when it captures a village. Unit that at the start of the turn is in the village heals 8 hp.

### Terrain

There are multiple types of terrains in the game. The type of terraing affects movement cost on this terrain and also unit's chance of getting hit. To see the stats of each unit visit: https://units.wesnoth.org/1.15/mainline/en_US/mainline.html

### Area of control

Each unit creates an area of control on adjacent tiles to it. That meaning if enemy unit were end its movement on the tile that is controlled, its MP would go to 0. That is to ensure that units cannot easily sneak into the backline killing important or wounded units.

### Fight of 2 units

When player attacks an enemy unit, they get to choose an attack with which it will attack. Player can see with what attack will the enemy unit defend itself. Melee attacks can be defended only by melee attacks and the same with ranged attacks. Player can see Damage x Number of hits. Starting with attacking unit both units try to hit the other (depending on the terraing and luck of course :D)  with their attack.

### XP

Killing an unit is for 8 xp per level of that unit. If the unit gains more than max xp it heals up to full health and if it can upgrade to a stronger unit it does so.

## Last words

I recommend the original Wesnoth game to anyone who is willing to try it. I've had hours of fun with it - that is why I tried to recreate the basics of it as a project.