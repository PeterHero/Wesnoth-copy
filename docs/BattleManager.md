# Battle Manager

BattleManager.cs handles turns, players, units, interaction between two units (fighting) and reacts to UI.

## UI

Manager opens recruit screen when 'r' is pressed and conditions are met. It also handles recruiting an unit, attack of an unit and the end of players turn - on players command (pressing a button). 

## Battle preparation

The method **PrepareBattle()** is used by game manager to create the starting point of the game - generate players with heroes.

## Other

Other methods are concerned about fight of 2 units and unit creation.