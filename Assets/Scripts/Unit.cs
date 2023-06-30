using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string UnitTypeName { get; set; }

    public int MaxHP { get; set; }
    private int currentHP;
    public int CurrentHP { get => currentHP; protected set { currentHP = value; } }
    public int MaxMovement { get; set; }
    public int CurrentMovement { get; set; }

    public Dictionary<Tile.TerrainType, int> movementCost = new Dictionary<Tile.TerrainType, int>();
    public int MaxXP { get; set; }
    public int CurrentXP { get; set; }

    public int Level { get; set; }

    public Dictionary<Tile.TerrainType, int> defence = new Dictionary<Tile.TerrainType, int>();
    public int Defence { get; set; }
    public int Resistence { get; set; }

    public List<Attack> Attacks = new List<Attack>();

    public Unit()
    {
        
    }

    // Reduces currentHP by damage done. Returns true if the Unit died, if it is alive return false.
    private bool takeDamage(int damage)
    {
        
        currentHP -= damage;
        if (CurrentHP <= 0)
            return true;
        else
            return false;
    }

    // Calculates how much damage the unit takes based on its defence and resistence
    public bool Attacked(Attack attack)
    {
        int random = Random.Range(1, 100);

        if (random > Defence) // is hit
        {
            return takeDamage(attack.damage * (100 - Resistence) / 100);
        }
        else
            return false;
    }

    public void setDefense(Tile.TerrainType terrain)
    {
        Defence = defence[terrain];
    }
}
