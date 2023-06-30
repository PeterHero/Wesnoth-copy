using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvishArcher : Unit
{
    public ElvishArcher()
    {
        UnitTypeName = "Elvish Archer";

        MaxHP = 29;
        CurrentHP = MaxHP;
        MaxMovement = 6;
        CurrentMovement = MaxMovement;
        movementCost.Add(Tile.TerrainType.castle, 1);
        movementCost.Add(Tile.TerrainType.flat, 1);
        movementCost.Add(Tile.TerrainType.forest, 1);
        movementCost.Add(Tile.TerrainType.hills, 2);
        movementCost.Add(Tile.TerrainType.village, 1);
        movementCost.Add(Tile.TerrainType.water, 3);

        MaxXP = 44;

        Level = 1;

        defence.Add(Tile.TerrainType.castle, 60);
        defence.Add(Tile.TerrainType.flat, 40);
        defence.Add(Tile.TerrainType.forest, 70);
        defence.Add(Tile.TerrainType.hills, 50);
        defence.Add(Tile.TerrainType.village, 60);
        defence.Add(Tile.TerrainType.water, 20);

        Resistence = 0;

        Attacks.Add(new Attack("sword", 5, 2, Attack.AttackForm.melee));

    }
}
