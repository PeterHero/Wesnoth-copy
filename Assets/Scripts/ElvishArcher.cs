using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvishArcher : Unit
{
    public ElvishArcher()
    {
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

        Defence = 50;
        Resistence = 0;

        Attacks.Add(new Attack("sword", 5, 2, Attack.AttackForm.melee));

    }
}
