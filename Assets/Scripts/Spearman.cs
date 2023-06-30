using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Unit
{
    public Spearman()
    {
        MaxHP = 36;
        CurrentHP = MaxHP;
        MaxMovement = 5;
        CurrentMovement = MaxMovement;
        movementCost.Add(Tile.TerrainType.castle, 1);
        movementCost.Add(Tile.TerrainType.flat, 1);
        movementCost.Add(Tile.TerrainType.forest, 2);
        movementCost.Add(Tile.TerrainType.hills, 2);
        movementCost.Add(Tile.TerrainType.village, 1);
        movementCost.Add(Tile.TerrainType.water, 3);

        MaxXP = 42;

        Level = 1;

        defence.Add(Tile.TerrainType.castle, 60);
        defence.Add(Tile.TerrainType.flat, 40);
        defence.Add(Tile.TerrainType.forest, 50);
        defence.Add(Tile.TerrainType.hills, 50);
        defence.Add(Tile.TerrainType.village, 60);
        defence.Add(Tile.TerrainType.water, 20);

        Resistence = 0;

        Attacks.Add(new Attack("spear", 7, 3, Attack.AttackForm.melee));
        
    }
}
